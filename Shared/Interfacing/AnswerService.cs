namespace Shared.Interfacing;

public interface IAnswerService
{
    Task<AnswerStatus> PostAnswerAsync(int year, int day,int dayPart, string answer);
}
public class AnswerService : IAnswerService
{
    private readonly IAoCClient _aoCClient;

    public AnswerService(IAoCClient aoCClient)
    {
        _aoCClient = aoCClient;
    }
    
    public async Task<AnswerStatus> PostAnswerAsync(int year, int day,int dayPart, string answer)
    {
        var result = await _aoCClient.PostAnswerForDayAsync(year, day, dayPart, answer);
        return result switch
        {
            _ when result.ToLower().Contains("that's the right answer!") => AnswerStatus.Correct,
            _ when result.ToLower().Contains("that's not the right answer") => AnswerStatus.Wrong,
            _ when result.ToLower().Contains("you gave an answer too recently") => AnswerStatus.TooSoon,
            _ when result.ToLower().Contains("you don't seem to be solving the right level.") => AnswerStatus.DayAlreadyCompleted,
            _ => AnswerStatus.Unknown
        };
        
    }
}
