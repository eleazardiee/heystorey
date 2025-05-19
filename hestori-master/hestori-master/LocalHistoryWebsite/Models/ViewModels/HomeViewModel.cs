using System.Collections.Generic;

namespace LocalHistoryWebsite.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<HistoryPost> Posts { get; set; } = new List<HistoryPost>();
        public HistoryPostViewModel CreatePostViewModel { get; set; }
    }
}