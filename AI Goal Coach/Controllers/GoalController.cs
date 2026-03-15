namespace AI_Goal_Coach.Controllers
{
    [ApiController]
    [Route("api/goals")]
    public class GoalController : ControllerBase
    {
        private readonly IGoalDomainLogic _goalDomain;

        public GoalController(IGoalDomainLogic goalDomain)
        {
            _goalDomain = goalDomain;
        }

        [HttpPost("refine")]
        public async Task<IActionResult> RefineGoal([FromBody] GoalRequest request)
        {
            var result = await _goalDomain.RefineGoalAsync(request.Goal);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult SaveGoal([FromBody] GoalResponse goal)
        {
            _goalDomain.SaveGoal(goal);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetGoals()
        {
            var goals = _goalDomain.GetGoals();

            return Ok(goals);
        }
    }
}
