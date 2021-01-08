/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID_19.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace COVID_19.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AppDbContext>>()))
            {
                // Look for any movies.
                if (context.Country.Any())
                {
                    return;   // DB has been seeded
                }

                context.Country.AddRange(
                     new Country
                     {
                         CountryName = "India",
                         Capital = "Delhi",
                         CountryCode = "IND",
                         Population = 1350000000,
                         HumanDevelopmentIndex_HDI = 129,
                         Density = 382,
                         DateUpdated = DateTime.Now,
                         Level = Country.DevelopmentLevel.Developing
                     }
                );

                if (context.SurveyQuestions.Any())
                {
                    return;
                }
                context.SurveyQuestions.AddRange(
                     new SurveyQuestions
                     {
                         Question = "Are you looking forward to returning to work",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     },
                     new SurveyQuestions
                     {
                         Question = "If you have been working from home, would you prefer to continue doing so?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     },
                     new SurveyQuestions
                     {
                         Question = "On a scale of 1 – 10, how would you rate your communication with your manager while working from home?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     },
                     new SurveyQuestions
                     {
                         Question = "Have you been equally as productive, less productive, or more productive while working from home?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     },
                     new SurveyQuestions
                     {
                         Question = "Do you have concerns about commuting to work?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     },
                     new SurveyQuestions
                     {
                         Question = "Are you nervous about returning to work while the threat of Covid-19 remains?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     },
                     new SurveyQuestions
                     {
                         Question = "Are you able to be flexible in your start, break, and finish times to account for social distancing " +
                         "and limiting the number of employees on-site at a single time?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     },
                     new SurveyQuestions
                     {
                         Question = "Would you be willing to wear a face mask?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     },
                     new SurveyQuestions
                     {
                         Question = "Are you willing to notify your employer if you encounter anyone – " +
                         "or go anywhere – with a high risk of Covid-19 infection?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.Social
                     },
                     new SurveyQuestions
                     {
                         Question = "Have you been diagnosed with Covid-19 while away from work?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     },
                     new SurveyQuestions
                     {
                         Question = "Do you have any additional concerns about returning to work?",
                         SurveyQuestionType = SurveyQuestions.SurveyType.WFH
                     }
                );

                if (context.UserMaster.Any())
                {
                    return;
                }
                context.UserMaster.AddRange(
                    new UserMaster
                    {
                        FirstName = "Avishek",
                        LastName = "Choudhury",
                        UserName = "helloworld",
                        Password = "helloworld",
                        Email = "choudhuryavishek2009@gmail.com",
                        DateOfBirth = DateTime.Now,
                        DateCreated = DateTime.Now,
                        Age = 24,
                        IsActive = true
                    });

                if (context.SurveyUserData.Any())
                {
                    return;
                }
                context.SurveyUserData.AddRange(
                    new SurveyUserData
                    {
                        UserId = 1,
                        SurveyDate = DateTime.Now,
                        SurveyQuestionsType = "WFH",
                        SurveyQuestion1 = true,
                        SurveyQuestion2 = true,
                        SurveyQuestion3 = true,
                        SurveyQuestion4 = true,
                        SurveyQuestion5 = true,
                        SurveyQuestion6 = true,
                        SurveyQuestion7 = true,
                        SurveyQuestion8 = true,
                        SurveyQuestion9 = true,
                        SurveyQuestion10 = true,
                        SurveyQuestion11 = "Concern is how are we going to adapt to the sudden change" +
                        "in the work environment for a sole worker in a silent room to an office floor with 50 people."
                    });

                context.SaveChanges();
            }
        }
    }
}*/
