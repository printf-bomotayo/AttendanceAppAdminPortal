using API.Entities;

namespace API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Candidates.Any()) return;

            var candidates = new List<Candidate>
            {
                new Candidate { FirstName = "Liam", LastName = "Smith", Email = "liam.smith@example.com", PhoneNumber = "123-456-7890", CohortId = 1 },
                new Candidate { FirstName = "Emma", LastName = "Johnson", Email = "emma.johnson@example.com", PhoneNumber = "987-654-3210", CohortId = 2 },
                new Candidate { FirstName = "Noah", LastName = "Williams", Email = "noah.williams@example.com", PhoneNumber = "555-555-5555", CohortId = 3 },
                new Candidate { FirstName = "Olivia", LastName = "Brown", Email = "olivia.brown@example.com", PhoneNumber = "444-444-4444", CohortId = 4 },
                new Candidate { FirstName = "William", LastName = "Jones", Email = "william.jones@example.com", PhoneNumber = "333-333-3333", CohortId = 1 },
                new Candidate { FirstName = "Ava", LastName = "Garcia", Email = "ava.garcia@example.com", PhoneNumber = "222-222-2222", CohortId = 2 },
                new Candidate { FirstName = "James", LastName = "Martinez", Email = "james.martinez@example.com", PhoneNumber = "111-111-1111", CohortId = 3 },
                new Candidate { FirstName = "Isabella", LastName = "Rodriguez", Email = "isabella.rodriguez@example.com", PhoneNumber = "000-000-0000", CohortId = 4 },
                new Candidate { FirstName = "Lucas", LastName = "Wilson", Email = "lucas.wilson@example.com", PhoneNumber = "123-123-1234", CohortId = 1 },
                new Candidate { FirstName = "Sophia", LastName = "Anderson", Email = "sophia.anderson@example.com", PhoneNumber = "321-321-4321", CohortId = 2 },
                new Candidate { FirstName = "Mason", LastName = "Thomas", Email = "mason.thomas@example.com", PhoneNumber = "456-456-6543", CohortId = 3 },
                new Candidate { FirstName = "Mia", LastName = "Taylor", Email = "mia.taylor@example.com", PhoneNumber = "654-654-3456", CohortId = 4 },
                new Candidate { FirstName = "Ethan", LastName = "Hernandez", Email = "ethan.hernandez@example.com", PhoneNumber = "789-789-9876", CohortId = 1 },
                new Candidate { FirstName = "Amelia", LastName = "Moore", Email = "amelia.moore@example.com", PhoneNumber = "987-987-6789", CohortId = 2 },
                new Candidate { FirstName = "Logan", LastName = "Martin", Email = "logan.martin@example.com", PhoneNumber = "234-234-5678", CohortId = 3 },
                new Candidate { FirstName = "Harper", LastName = "Jackson", Email = "harper.jackson@example.com", PhoneNumber = "876-876-1234", CohortId = 4 },
                new Candidate { FirstName = "Elijah", LastName = "Thompson", Email = "elijah.thompson@example.com", PhoneNumber = "345-345-2345", CohortId = 1 },
                new Candidate { FirstName = "Evelyn", LastName = "White", Email = "evelyn.white@example.com", PhoneNumber = "543-543-3456", CohortId = 2 },
                new Candidate { FirstName = "Alexander", LastName = "Lopez", Email = "alexander.lopez@example.com", PhoneNumber = "678-678-4567", CohortId = 3 },
                new Candidate { FirstName = "Abigail", LastName = "Lee", Email = "abigail.lee@example.com", PhoneNumber = "876-876-5678", CohortId = 4 },
                new Candidate { FirstName = "Henry", LastName = "Gonzalez", Email = "henry.gonzalez@example.com", PhoneNumber = "789-789-6789", CohortId = 1 },
                new Candidate { FirstName = "Ella", LastName = "Harris", Email = "ella.harris@example.com", PhoneNumber = "987-987-7890", CohortId = 2 },
                new Candidate { FirstName = "Sebastian", LastName = "Clark", Email = "sebastian.clark@example.com", PhoneNumber = "345-345-8901", CohortId = 3 },
                new Candidate { FirstName = "Grace", LastName = "Lewis", Email = "grace.lewis@example.com", PhoneNumber = "567-567-1234", CohortId = 4 },
                new Candidate { FirstName = "Aiden", LastName = "Robinson", Email = "aiden.robinson@example.com", PhoneNumber = "678-678-2345", CohortId = 1 },
                new Candidate { FirstName = "Chloe", LastName = "Walker", Email = "chloe.walker@example.com", PhoneNumber = "789-789-3456", CohortId = 2 },
                new Candidate { FirstName = "Matthew", LastName = "Perez", Email = "matthew.perez@example.com", PhoneNumber = "890-890-4567", CohortId = 3 },
                new Candidate { FirstName = "Sofia", LastName = "Hall", Email = "sofia.hall@example.com", PhoneNumber = "901-901-5678", CohortId = 4 },
                new Candidate { FirstName = "David", LastName = "Young", Email = "david.young@example.com", PhoneNumber = "123-123-6789", CohortId = 1 },
                new Candidate { FirstName = "Emily", LastName = "Allen", Email = "emily.allen@example.com", PhoneNumber = "234-234-7890", CohortId = 2 },
                new Candidate { FirstName = "Joseph", LastName = "King", Email = "joseph.king@example.com", PhoneNumber = "345-345-8901", CohortId = 3 },
                new Candidate { FirstName = "Avery", LastName = "Wright", Email = "avery.wright@example.com", PhoneNumber = "456-456-9012", CohortId = 4 },
                new Candidate { FirstName = "Jackson", LastName = "Scott", Email = "jackson.scott@example.com", PhoneNumber = "567-567-0123", CohortId = 1 },
                new Candidate { FirstName = "Scarlett", LastName = "Torres", Email = "scarlett.torres@example.com", PhoneNumber = "678-678-1234", CohortId = 2 },
                new Candidate { FirstName = "Samuel", LastName = "Nguyen", Email = "samuel.nguyen@example.com", PhoneNumber = "789-789-2345", CohortId = 3 },
                new Candidate { FirstName = "Aria", LastName = "Hill", Email = "aria.hill@example.com", PhoneNumber = "890-890-3456", CohortId = 4 },
                new Candidate { FirstName = "Owen", LastName = "Flores", Email = "owen.flores@example.com", PhoneNumber = "901-901-4567", CohortId = 1 },
                new Candidate { FirstName = "Isabelle", LastName = "Green", Email = "isabelle.green@example.com", PhoneNumber = "123-123-5678", CohortId = 2 },
                new Candidate { FirstName = "Jack", LastName = "Adams", Email = "jack.adams@example.com", PhoneNumber = "234-234-6789", CohortId = 3 },
                new Candidate { FirstName = "Lily", LastName = "Baker", Email = "lily.baker@example.com", PhoneNumber = "345-345-7890", CohortId = 4 },
                new Candidate { FirstName = "Wyatt", LastName = "Gonzales", Email = "wyatt.gonzales@example.com", PhoneNumber = "456-456-8901", CohortId = 1 },
                new Candidate { FirstName = "Hannah", LastName = "Nelson", Email = "hannah.nelson@example.com", PhoneNumber = "567-567-9012", CohortId = 2 },
                new Candidate { FirstName = "Daniel", LastName = "Carter", Email = "daniel.carter@example.com", PhoneNumber = "678-678-0123", CohortId = 3 },
                new Candidate { FirstName = "Zoey", LastName = "Mitchell", Email = "zoey.mitchell@example.com", PhoneNumber = "789-789-1234", CohortId = 4 },
                new Candidate { FirstName = "Gabriel", LastName = "Perez", Email = "gabriel.perez@example.com", PhoneNumber = "890-890-2345", CohortId = 1 },
                new Candidate { FirstName = "Victoria", LastName = "Roberts", Email = "victoria.roberts@example.com", PhoneNumber = "901-901-3456", CohortId = 2 },
                new Candidate { FirstName = "Carter", LastName = "Turner", Email = "carter.turner@example.com", PhoneNumber = "123-123-4567", CohortId = 3 },
                new Candidate { FirstName = "Nora", LastName = "Phillips", Email = "nora.phillips@example.com", PhoneNumber = "234-234-5678", CohortId = 4 },


            };
            
            context.Candidates.AddRange(candidates);

            // foreach (var candidate in candidates)
            // {
            //     context.Candidates.Add(candidate);
            // }

            context.SaveChanges();
        } 
    }
}