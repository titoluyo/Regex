using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using Generic.Text.RegularExpressions;
using System.Text;

public partial class RegexFunctions
{

    public static readonly RegexOptions Options = RegexOptions.Multiline;

    [SqlFunction]
    public static SqlBoolean RegexMatch(SqlChars input, SqlString pattern)
    {
        SqlBoolean result;
        try
        {
            var regex = new Regex(pattern.Value, Options);
            result = new SqlBoolean(regex.IsMatch(new string(input.Value)));
        }
        catch
        {
            result = new SqlBoolean(false);
        }
        return result;
    }

    [SqlFunction]
    public static SqlBoolean RegexMatch(SqlChars input, SqlString pattern, SqlInt32 timeout)
    {
        SqlBoolean result;
        try
        {
            var regex = new Regex(pattern.Value, Options, new TimeSpan(0, 0, 0, 0, timeout.Value));
            result = regex.IsMatch(new string(input.Value));
        }
        catch
        {
            result = new SqlBoolean(false);
        }
        return result;
    }

    [SqlFunction]
    public static SqlString RegexReplace(SqlString expression, SqlString pattern, SqlString replace)
    {
        if (expression.IsNull || pattern.IsNull || replace.IsNull)
            return SqlString.Null;

        Regex r = new Regex(pattern.ToString());
        return new SqlString(r.Replace(expression.ToString(), replace.ToString()));
    }

    // returns the matching string. Results are separated by 3rd parameter
    [SqlFunction]
    public static SqlString RegexSelectAll(SqlChars input, SqlString pattern, SqlString matchDelimiter)
    {
        Regex regex = new Regex(pattern.Value, Options);
        Match results = regex.Match(new string(input.Value));

        StringBuilder sb = new StringBuilder();
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
        Regex regex = new Regex(pattern.Value, Options);
        Match results = regex.Match(new string(input.Value));

        string resultStr = "";
        int index = 0;

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

};