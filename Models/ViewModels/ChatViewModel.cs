namespace SignalRSample.Models.ViewModels
{
    public class ChatViewModel
    {
        public ChatViewModel()
        {
            Rooms = new List<ChatRoom>();
        }

        public IList<ChatRoom> Rooms { get; set; }
        public int MaxRoomAllowed { get; set; }
        public string? UserId { get; set; }
        public bool AllowAddRoom => Rooms == null || Rooms.Count < MaxRoomAllowed;
    }
}
