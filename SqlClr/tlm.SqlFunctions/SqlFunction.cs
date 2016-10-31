using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    public static readonly IDictionary<int, Regex> Patterns = new Dictionary<int,Regex>();
    public static readonly Random rnd = new Random();
    public static readonly int val = rnd.Next(1000, 9999);

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlFunction(SqlString str1, SqlString str2)
    {
        return new SqlString("+" + str1.Value + "|" + str2.Value + "+");
    }

    public static readonly RegexOptions Options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline;

    [SqlFunction]
    public static SqlBoolean RegexMatch(SqlChars input, SqlString pattern)
    {
        try
        {
            var regex = new Regex(pattern.Value, Options);
            return regex.IsMatch(new string(input.Value));
        }
        catch
        {
            return false;
        }
    }

    [SqlFunction]
    public static SqlString Test(SqlInt32 id, SqlChars input, SqlString pattern)
    {
        var found = true;
        try
        {
            var i = id.Value;
            if (!Patterns.ContainsKey(i))
            {
                found = false;
                Patterns[i] = new Regex(pattern.Value, Options | RegexOptions.Compiled);
            }
            return string.Format("{0} - {1} - {2}", i, found, Patterns[i].IsMatch(new string(input.Value)));
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [SqlFunction]
    public static SqlString RegexReplace(SqlString expression, SqlString pattern, SqlString replace)
    {
        if (expression.IsNull || pattern.IsNull || replace.IsNull)
            return SqlString.Null;

        var r = new Regex(pattern.ToString());
        return new SqlString(r.Replace(expression.ToString(), replace.ToString()));
    }

    // returns the matching string. Results are separated by 3rd parameter
    [SqlFunction]
    public static SqlString RegexSelectAll(SqlChars input, SqlString pattern, SqlString matchDelimiter)
    {
        var regex = new Regex(pattern.Value, Options);
        var results = regex.Match(new string(input.Value));

        var sb = new StringBuilder();
        while (results.Success)
        {
            sb.Append(results.Value);

            results = results.NextMatch();

            // separate the results with newline|newline
            if (results.Success)
            {
                sb.Append(matchDelimiter.Value);
            }
        }

        return new SqlString(sb.ToString());

    }

    // returns the matching string
    // matchIndex is the zero-based index of the results. 0 for the 1st match, 1, for 2nd match, etc
    [SqlFunction]
    public static SqlString RegexSelectOne(SqlChars input, SqlString pattern, SqlInt32 matchIndex)
    {
        var regex = new Regex(pattern.Value, Options);
        var results = regex.Match(new string(input.Value));

        var resultStr = "";
        var index = 0;

        while (results.Success)
        {
            if (index == matchIndex)
            {
                resultStr = results.Value.ToString();
            }

            results = results.NextMatch();
            index++;

        }

        return new SqlString(resultStr);

    }


}
