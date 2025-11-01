namespace SportNewsAndScores.Core.Interfaces;

public interface IAICommentService
{
    Task<string> GenerateCommentAsync(string content, string provider);
}
