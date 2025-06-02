using GenerativeAI.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetEstimation(int user_id)
        {
            var response = new EstimationProfileDto
            {
                CountWorks = CountSendWorks(user_id),
                CountRatedExc = RatedExcellent(user_id),
                CountRatedGood = RatedGood(user_id),
                CountRatedSatisfactory = RatedSatisfactory(user_id),
                CountRatedUnSatisfactory = RatedUnSatisfactory(user_id)
            };

            return Ok(response);
        }
        [Authorize]
        [HttpGet("get-estimations-all-groups", Name = "GetEstimationsAllGroups")]
        public IActionResult GetEstimationsAllGroups()
        {
            var est_ids = _context.Files.Select(f => f.EstimationId).ToArray();
            var estimations = _context.Estimations.Where(e => est_ids.Contains(e.EstimationId));
            var countEstEx = estimations.Where(e => e.EstContent + e.EstRelevance + e.EstStylistic >= 81).Count();
            var countEstGood = estimations.Where(e => e.EstContent + e.EstRelevance + e.EstStylistic >= 61 &&
            e.EstContent + e.EstRelevance + e.EstStylistic < 81)
                .Count();
            var countEstSutisfactory = estimations.Where(e => e.EstContent + e.EstRelevance + e.EstStylistic >= 41 &&
            e.EstContent + e.EstRelevance + e.EstStylistic < 61)
                .Count();
            var countEstUnSutisfactory = estimations.Where(e => e.EstContent + e.EstRelevance + e.EstStylistic < 41).Count();
            int[] arrayEstimationsCount = [countEstEx, countEstGood, countEstSutisfactory, countEstUnSutisfactory];
            return Ok(arrayEstimationsCount);
        }
        private int CountSendWorks(int user_id)
        {
            var sendWorks = from f in _context.Files
                            where f.UserId == user_id
                            select f;
            var sendWorks1 = _context.Users.FirstOrDefault(u => u.Id == user_id);
            return sendWorks.Count();
        }
        private int RatedExcellent(int user_id)
        {
            var ratedExcellent = from f in _context.Files
                                 join e in _context.Estimations
                                 on f.EstimationId equals e.EstimationId
                                 where f.UserId == user_id &&
                                 e.EstContent + e.EstRelevance + e.EstStylistic >= 81
                                 select f;
            return ratedExcellent.Count();
        }
        private int RatedGood(int user_id)
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
            return ratedGood.Count();
        }
        private int RatedSatisfactory(int user_id)
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
            return ratedSatisfactory.Count();
        }
        private int RatedUnSatisfactory(int user_id)
        {
            var ratedUnSatisfactory = from f in _context.Files
                                      join e in _context.Estimations
                                      on f.EstimationId equals e.EstimationId
                                      where f.UserId == user_id
                                      &&
                                      e.EstContent + e.EstRelevance + e.EstStylistic < 41
                                      select f;
            return ratedUnSatisfactory.Count();
        }

    }
}
