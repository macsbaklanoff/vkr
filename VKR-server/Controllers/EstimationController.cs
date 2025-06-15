
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using VKR_server.DB;
using VKR_server.Dto;

namespace VKR_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstimationController : ControllerBase
    {
        private readonly ILogger<EstimationController> _logger;

        private readonly ApplicationContext _context;

        public EstimationController(ILogger<EstimationController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Authorize]
        [HttpGet("get_estimation/{user_id}", Name = "GetEstimation")]
        public async Task<IActionResult> GetEstimation(int user_id)
        {
            var response = new EstimationProfileDto
            {
                CountWorks = CountSendWorks(user_id),
                CountRatedExc = await RatedExcellent(user_id),
                CountRatedGood = await RatedGood(user_id),
                CountRatedSatisfactory = await RatedSatisfactory(user_id),
                CountRatedUnSatisfactory = await RatedUnSatisfactory(user_id)
            };

            return Ok(response);
        }

        //метаданные файла и оценки
        [HttpGet("get-info-file-estimation/{user_id}", Name = "GetInfoFileEstimation")]
        [Authorize]
        public async Task<IActionResult> GetInfoFileEstimation(int user_id)
        {
            var files = from f in _context.Files
                        where f.UserId == user_id
                        join e in _context.Estimations
                        on f.EstimationId equals e.EstimationId
                        select new
                        {
                            fileName = f.FileName,
                            academicSubject = f.AcademicSubject,
                            topicWork = f.TopicWork,
                            estContent = e.EstContent,
                            estRelevance = e.EstRelevance,
                            estStylistic = e.EstStylistic,
                            estRecommendations = e.EstRecommedations
                        };

            return Ok(files.ToList());
        }

        [Authorize]
        [HttpGet("get-estimations-all-groups", Name = "GetEstimationsAllGroups")]
        public IActionResult GetEstimationsAllGroups()
        {
            int[] arrayEstimationsCount = [];
            int countEstEx = 0, countEstGood = 0, countEstSutisfactory = 0, countEstUnSutisfactory = 0;
            var est_ids = _context.Files.Where(f => f.User.Group != null).Select(f => f.EstimationId).ToArray();

            var estimations = _context.Estimations.Where(e => est_ids.Contains(e.EstimationId));
            countEstEx = estimations.Where(e => e.EstContent + e.EstRelevance + e.EstStylistic >= 81).Count();
            countEstGood = estimations.Where(e => e.EstContent + e.EstRelevance + e.EstStylistic >= 61 &&
            e.EstContent + e.EstRelevance + e.EstStylistic < 81)
                .Count();
            countEstSutisfactory = estimations.Where(e => e.EstContent + e.EstRelevance + e.EstStylistic >= 41 &&
            e.EstContent + e.EstRelevance + e.EstStylistic < 61)
               .Count();
            countEstUnSutisfactory = estimations.Where(e => e.EstContent + e.EstRelevance + e.EstStylistic < 41).Count();

            arrayEstimationsCount = [countEstEx, countEstGood, countEstSutisfactory, countEstUnSutisfactory];
            return Ok(arrayEstimationsCount);
        }
        [Authorize]
        [HttpGet("get-estimations-group/{group_id}", Name = "GetEstimationsGroup")]
        public async Task<IActionResult> GetEstimationsGroup(int group_id)
        {
            int[] arrayEstimationsCount = [];
            var usersInGroup = await _context.Users.Where(u => u.Group.GroupId == group_id).ToListAsync();
            int countEstEx = 0, countEstGood = 0, countEstSutisfactory = 0, countEstUnSutisfactory = 0;
            foreach (var user in usersInGroup)
            {
                countEstEx += await RatedExcellent(user.Id);
                countEstGood += await RatedGood(user.Id);
                countEstSutisfactory += await RatedSatisfactory(user.Id);
                countEstUnSutisfactory += await RatedUnSatisfactory(user.Id);
            }
            arrayEstimationsCount = [countEstEx, countEstGood, countEstSutisfactory, countEstUnSutisfactory];
            return Ok(arrayEstimationsCount);
        }
        private int CountSendWorks(int user_id)
        {
            var sendWorks = from f in _context.Files
                            where f.UserId == user_id
                            select f;
            return sendWorks.Count();
        }
        private async Task<int> RatedExcellent(int user_id)
        {
            var ratedExcellent = from f in _context.Files
                                 join e in _context.Estimations
                                 on f.EstimationId equals e.EstimationId
                                 where f.UserId == user_id &&
                                 e.EstContent + e.EstRelevance + e.EstStylistic >= 81
                                 select f;
            return await ratedExcellent.CountAsync();
        } 
        private async Task<int> RatedGood(int user_id)
        {
            var ratedGood = from f in _context.Files
                            join e in _context.Estimations
                            on f.EstimationId equals e.EstimationId
                            where f.UserId == user_id 
                            &&
                            e.EstContent + e.EstRelevance + e.EstStylistic >= 61
                            &&
                            e.EstContent + e.EstRelevance + e.EstStylistic < 81
                            select f;
            return await ratedGood.CountAsync();
        }
        private async Task<int> RatedSatisfactory(int user_id)
        {
            var ratedSatisfactory = from f in _context.Files
                                    join e in _context.Estimations
                                    on f.EstimationId equals e.EstimationId
                                    where f.UserId == user_id
                                    &&
                                    e.EstContent + e.EstRelevance + e.EstStylistic >= 41
                                    &&
                                    e.EstContent + e.EstRelevance + e.EstStylistic < 61
                                    select f;
            return await ratedSatisfactory.CountAsync();
        }
        private async Task<int> RatedUnSatisfactory(int user_id)
        {
            var ratedUnSatisfactory = from f in _context.Files
                                      join e in _context.Estimations
                                      on f.EstimationId equals e.EstimationId
                                      where f.UserId == user_id
                                      &&
                                      e.EstContent + e.EstRelevance + e.EstStylistic < 41
                                      select f;
            return await ratedUnSatisfactory.CountAsync();
        }

    }
}
