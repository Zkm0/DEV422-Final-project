namespace PlayerManagementService.Models
{
    // Player model 
    public class Player
    {
        public int PlayerId { get; set; }   // Unique ID for the player
        public string Name { get; set; } = string.Empty;    // Player's name
        public string Position { get; set; } = string.Empty; // e.g. "Forward", "Guard", etc.

        // TeamId is nullable because a player might not be drafted yet
        public int? TeamId { get; set; }

        public bool IsDrafted { get; set; } // True if the player is on a team
    }
}
