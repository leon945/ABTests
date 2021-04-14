using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCodes
{
	public const int SUCCESS = 0;
	public const int ACCOUNT_CREATED = 100;
	public const int LOGIN_SUCCESS = 101;
	public const int USER_ADDED = 102;

	//200-249 - SUCCESS	
	public const int GAME_CREATED = 200;
	public const int MEMBER_ADDED = 201;
	public const int GAME_STARTED = 202;
	public const int GAME_ENDED = 203;

	//150-199 - ERROR
	public const int _INVALID_EMAIL = 150;
	public const int _EMAIL_IN_USE = 151;
	public const int _USERNAME_UNAVAILABLE = 152;
	public const int _USERNAME_OVER_30 = 153;
	public const int _NO_PASSWORD = 154;
	public const int _WRONG_LOGIN = 155;
	public const int _COMMA_IN_USERNAME = 156;
	public const int _INVALID_EMAIL_OR_PASSWORD = 157;
	public const int _INVALID_LANG = 158;
	public const int _USER_NOT_FOUND = 159;
	public const int _USER_ALREADY_EXISTS = 160;
}
