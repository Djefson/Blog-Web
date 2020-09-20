namespace BlogEngine6.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogEngine6.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BlogEngine6.Models.ApplicationDbContext context)
        {

            var blogs = new List<Blog>
            {
                new Blog
                {
                    UserID = context.Users.Single(u => u.Email == "test@example.com").Id,
                    PostDate =  DateTime.ParseExact("20/12/2016 13:45:00", "dd/MM/yyyy HH:mm:ss",null),
                    Title = "My first blog!",
                    Content = "This is an example blog."
                },
                new Blog
                {
                    UserID = context.Users.Single(u => u.Email == "test@example.com").Id,
                    PostDate = DateTime.Now,
                    Title = "My darkest secret...",
                    Content = "I prefer raisin cookies over chocolate chip."
                },
                new Blog
                {
                    UserID = context.Users.Single(u => u.Email == "test2@example.com").Id,
                    PostDate = DateTime.Now,
                    Title = "Hello World!",
                    Content = getBlog1()
                }
            };

            blogs.ForEach(s => context.Blogs.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();


            var tags = new List<Tag>
            {
                new Tag
                {
                    Name = "Humor"
                },
                new Tag
                {
                    Name = "Business"
                },
                new Tag
                {
                    Name = "Creativity"
                },
                new Tag
                {
                    Name = "Government"
                },
                new Tag
                {
                    Name = "Innovation"
                },
                new Tag
                {
                    Name = "Life Lessons"
                },
                new Tag
                {
                    Name = "Programming"
                },
                new Tag
                {
                    Name = "Self Improvement"
                },
                new Tag
                {
                    Name = "Tech"
                }
            };

            tags.ForEach(s => context.Tags.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

        }

        private string getBlog1()
        {



            return "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Primum in nostrane potestate est, quid meminerimus? " +
                   "Non autem hoc: igitur ne illud quidem. Quaero igitur, quo modo hae tantae commendationes a natura profectae subito " +
                   "a sapientia relictae sint. <b>Istam voluptatem, inquit, Epicurus ignorat?</b> Idem iste, inquam, de voluptate quid "+
                   "sentit? Duarum enim vitarum nobis erunt instituta capienda. Duo Reges: constructio interrete. Si longus, levis dictata sunt. </p>" +
                   "<p>Sint ista Graecorum; Compensabatur, inquit, cum summis doloribus laetitia. <b>Occultum facinus esse potuerit, gaudebit;</b> "+ 
                   "Philosophi autem in suis lectulis plerumque moriuntur. Non modo carum sibi quemque, verum etiam vehementer carum esse? </p>" +
                   "<p>Sed quid attinet de rebus tam apertis plura requirere? <b>Quid Zeno?</b> <mark>Quis Aristidem non mortuum diligit?</mark> "+
                   "Nam, ut sint illa vendibiliora, haec uberiora certe sunt. </p>" +
                   "<p>Illud non continuo, ut aeque incontentae. Videamus igitur sententias eorum, tum ad verba redeamus. <i>Quid de Pythagora?</i> "+
                   "<a href='http://loripsum.net/' target='_blank'>Inde igitur, inquit, ordiendum est.</a> Bonum liberi: misera orbitas. "+
                   "Potius inflammat, ut coercendi magis quam dedocendi esse videantur. </p>" +
                   "<p><a href='http://loripsum.net/' target='_blank'>Sed ille, ut dixi, vitiose.</a> Utinam quidem dicerent alium alio beatiorem! "+
                   "Iam ruinas videres. Sequitur disserendi ratio cognitioque naturae; Illa videamus, quae a te de amicitia dicta sunt. "+
                   "<a href='http://loripsum.net/' target='_blank'>Ita nemo beato beatior.</a> Sed haec omittamus; Haec para/doca illi, "+
                   "nos admirabilia dicamus. Gerendus est mos, modo recte sentiat. </p>" +

                    "<ul>" +
                        "<li>Non modo carum sibi quemque, verum etiam vehementer carum esse?</li>" +
                        "<li>Tum Piso: Quoniam igitur aliquid omnes, quid Lucius noster?</li>" +
                        "<li>Quam ob rem tandem, inquit, non satisfacit?</li>" +
                        "<li>Est autem etiam actio quaedam corporis, quae motus et status naturae congruentis tenet;</li>" +
                    "</ul>" +

                    "<ol>" +
                        "<li>Nam Pyrrho, Aristo, Erillus iam diu abiecti.</li>" +
                        "<li>-, sed ut hoc iudicaremus, non esse in iis partem maximam positam beate aut secus vivendi.</li>" +
                        "<li>Tu autem negas fortem esse quemquam posse, qui dolorem malum putet.</li>" +
                        "<li>Tum ille timide vel potius verecunde: Facio, inquit.</li>" +
                        "<li>Id enim natura desiderat.</li>" +
                        "<li>Quod maxime efficit Theophrasti de beata vita liber, in quo multum admodum fortunae datur.</li>" +
                    "</ol>";


        }

    }
}

