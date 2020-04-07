namespace Jugnoon.Models
{
    // User Profile Navigation Model View
    public class NavModelView
    {
        public int ActiveIndex { get; set; } = 0;
        public string username { get; set; } = "";
        public bool ShowVideos { get; set; } = false;
        public bool ShowAudio { get; set; } = false;
        public bool ShowPhotos { get; set; } = false;
        public bool Showqa { get; set; } = false;
        public bool ShowForumPosts { get; set; } = false;

        public int CountVideos { get; set; } = 0;
        public int CountAudio { get; set; } = 0;
        public int CountPhotos { get; set; } = 0;
        public int Countqa { get; set; } = 0;
        public int CountTopics { get; set; } = 0;

    }
}