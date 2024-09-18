using API.Entities;
using API.Repository.CohortRepo;
using API.Repository.TrainingProgramRepo;

namespace API.Services.TrainingProgramService
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly ITrainingProgramRepository _trainingProgramRepository;
        private readonly ICohortRepository _cohortRepository;

        public TrainingProgramService(ITrainingProgramRepository trainingProgramRepository, ICohortRepository cohortRepository)
        {
            _trainingProgramRepository = trainingProgramRepository;
            _cohortRepository = cohortRepository;
        }

        public async Task<TrainingProgram> GetTrainingProgramByIdAsync(int id)
        {
            return await _trainingProgramRepository.GetTrainingProgramByIdAsync(id);
        }

        public async Task AddCohortToTrainingProgramAsync(int trainingProgramId, Cohort cohort)
        {
            var trainingProgram = await _trainingProgramRepository.GetTrainingProgramByIdAsync(trainingProgramId);
            if (trainingProgram == null)
            {
                throw new KeyNotFoundException("Training Program not found");
            }

            cohort.TrainingProgramId = trainingProgramId;
            trainingProgram.CohortList.Add(cohort);

            await _cohortRepository.AddAsync(cohort);
            await _trainingProgramRepository.UpdateTrainingProgramAsync(trainingProgram);
        }
        public async Task<TrainingProgram> GetByIdAsync(int id)
        {
            return await _trainingProgramRepository.GetByIdAsync(id);
        }

        public async Task<List<TrainingProgram>> GetAllAsync()
        {
            return await _trainingProgramRepository.GetAllAsync();
        }

        public async Task AddAsync(TrainingProgram trainingProgram)
        {
            await _trainingProgramRepository.AddAsync(trainingProgram);
        }

        public async Task UpdateAsync(TrainingProgram trainingProgram)
        {
            await _trainingProgramRepository.UpdateAsync(trainingProgram);
        }

        public async Task DeleteAsync(int id)
        {
            await _trainingProgramRepository.DeleteAsync(id);
        }
    }
}