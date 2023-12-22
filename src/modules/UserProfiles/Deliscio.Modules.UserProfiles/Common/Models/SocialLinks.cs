using System.Diagnostics;

namespace Deliscio.Modules.UserProfiles.Common.Models;

public class SocialLinks
{
    public const string FACEBOOK = "Facebook";
    public const string GITHUB = "GitHub";
    public const string INSTAGRAM = "Instagram";
    public const string LINKED_IN = "LinkedIn";
    public const string PINTEREST = "Pinterest";
    public const string REDDIT = "Reddit";
    public const string SNAPCHAT = "Snapchat";
    public const string SPOTIFY = "Spotify";
    public const string TIKTOK = "TikTok";
    public const string TWITCH = "Twitch";
    public const string TWITTER = "Twitter";
    public const string YOUTUBE = "YouTube";

    public IEnumerable<string> List()
    {
        yield return FACEBOOK;
        yield return GITHUB;
        yield return INSTAGRAM;
        yield return LINKED_IN;
        yield return PINTEREST;
        yield return REDDIT;
        yield return SNAPCHAT;
        yield return SPOTIFY;
        yield return TIKTOK;
        yield return TWITCH;
        yield return TWITTER;
        yield return YOUTUBE;
    }
}