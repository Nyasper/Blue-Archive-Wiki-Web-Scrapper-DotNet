﻿namespace Main.FileHandler.Creator;

public interface ICreator
{
	Task<string> GenerateJsonData();
	Task<string> GenerateHtmlImagePreview();
}