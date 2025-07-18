﻿namespace UCStatistics.Shared.DTOs
{
    public class OfficeInfo
    {
        public int OfficeNr { get; set; }
        public string? OfficeName { get; set; } = string.Empty;
        public int Level2Nr { get; set; }
        public string? Level2Name { get; set; } = string.Empty;
        public int Level3Nr { get; set; }
        public string? Level3Name { get; set; } = string.Empty;
    }
}
