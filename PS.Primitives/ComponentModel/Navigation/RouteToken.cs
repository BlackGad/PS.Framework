﻿namespace PS.ComponentModel.Navigation
{
    internal class RouteToken
    {
        public readonly int Hash;
        public readonly int Token;
        public readonly string TokenString;
        public readonly string Value;

        #region Constructors

        public RouteToken(string value, int hash, int token)
        {
            Value = value;
            Hash = hash;
            Token = token;
            TokenString = ";" + token + ";";
        }

        #endregion

        #region Override members

        public override int GetHashCode()
        {
            return Hash;
        }

        public override string ToString()
        {
            return $"{Token} {Value}";
        }

        #endregion
    }
}