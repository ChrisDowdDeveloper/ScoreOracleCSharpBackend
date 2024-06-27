using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Services
{
    public class ScoreUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ScoreUpdateService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                    await UpdateScoresForCompletedGames(dbContext);
                }

                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }

        private async Task UpdateScoresForCompletedGames(ApplicationDBContext dbContext)
        {
            var completedGames = await dbContext.Games
                .Include(g => g.GamePrediction)
                .Where(g => g.GameStatus == GameStatus.COMPLETED && !g.ScoresUpdated)
                .ToListAsync();

            foreach (var game in completedGames)
            {
                foreach (var prediction in game.GamePrediction)
                {
                    if (string.IsNullOrEmpty(prediction.UserId)) continue;

                    int pointsAwarded = 0;

                    // Check if the outcome is correctly predicted (win/lose/draw).
                    bool isOutcomeCorrect = IsOutcomeCorrect(prediction, game);
                    if (isOutcomeCorrect) {
                        pointsAwarded += 1;
                    }

                    // Check if both scores are exactly correct.
                    bool areScoresExact = prediction.PredictedHomeTeamScore == game.HomeTeamScore &&
                                        prediction.PredictedAwayTeamScore == game.AwayTeamScore;
                    if (areScoresExact) {
                        pointsAwarded += 1;
                    }

                    // Update or create the user score record
                    if (pointsAwarded > 0) {
                        var userScore = dbContext.UserScores.FirstOrDefault(us => us.UserId == prediction.UserId);
                        if (userScore != null) {
                            userScore.Score += pointsAwarded;
                        } else {
                            dbContext.UserScores.Add(new UserScore {
                                UserId = prediction.UserId,
                                Score = pointsAwarded,
                                UpdatedLast = DateTime.UtcNow
                            });
                        }
                    }
                }

                game.ScoresUpdated = true;
            }

            await dbContext.SaveChangesAsync();
        }

        private bool IsOutcomeCorrect(Prediction prediction, Game game)
        {
            // Determine the result based on scores
            int homeScore = game.HomeTeamScore;
            int awayScore = game.AwayTeamScore;
            int predictedHomeScore = prediction.PredictedHomeTeamScore;
            int predictedAwayScore = prediction.PredictedAwayTeamScore;

            // Check if both predict the same outcome (win, lose, draw)
            bool actualHomeWin = homeScore > awayScore;
            bool predictedHomeWin = predictedHomeScore > predictedAwayScore;
            bool actualAwayWin = awayScore > homeScore;
            bool predictedAwayWin = predictedAwayScore > predictedHomeScore;
            bool actualDraw = homeScore == awayScore;
            bool predictedDraw = predictedHomeScore == predictedAwayScore;

            return (actualHomeWin && predictedHomeWin) || 
                (actualAwayWin && predictedAwayWin) ||
                (actualDraw && predictedDraw);
        }


    }
}