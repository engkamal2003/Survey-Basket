﻿using System;
namespace SurveyBasket.Api.Authentication
{
	public interface IJwtProvider
	{
		(string token, int expiresIn)GenerateToken(ApplicationUser user);
    }
}

