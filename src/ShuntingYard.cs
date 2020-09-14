using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleCalc
{
    // Logic for converting calculator input to an actual result
    public static class ShuntingYard
    {
        // Operators with priority, used for converting to Reverse Polish Notation
        private static readonly Dictionary<string, int> _operators = new Dictionary<string, int>()
        {
            { "+", 0 },
            { "–", 0 },
            { "*", 1 },
            { "/", 1 },
        };
        /// <summary>
        /// Converts an infix string to a postfix string(Reverse Polish Notation).
        /// </summary>
        /// <param name="infix">The calculator input.</param>
        public static List<string> ToPostFix(this string infix)
        {
            var splitInfix = StrArrayFromCalcInput(infix);
            var opStack = new Stack<string>(); // Where the operators will be stored e.g. ()+–
            var output = new List<string>(); // Where the final postfix output will be
            foreach (var token in splitInfix)
            {
                if (Decimal.TryParse(token, out _))
                    output.Add(token);
                else if (_operators.TryGetValue(token, out int currentPrio))
                {
                    while (opStack.Count > 0 && _operators.TryGetValue(opStack.Peek(), out int stackTopPrio))
                    {
                        if (currentPrio == stackTopPrio)
                            output.Add(opStack.Pop());
                        else
                            break; 
                    }
                    opStack.Push(token); // Add if stack is empty or operator isn't equal
                }
                else if (token == "(")
                    opStack.Push(token);
                else if (token == ")")
                {
                    var top = String.Empty;
                    while (opStack.Count > 0 && (top = opStack.Pop()) != "(") // Pop the stack until we find the closing paren
                        output.Add(top);
                    if (top != "(") // If the final char is a closing paren, we already popped (discarded) and we may continue
                        throw new ArgumentException("Syntax error"); // If not, a closing paren was never found
                }
                else
                    throw new ArgumentException("Syntax error"); // Invalid operator placement, or somehow invalid characters were injected
            }
            // Now we are left with just the remaining operators on the stack
            while (opStack.Count > 0)
            {
                var top = opStack.Pop();
                if (top != "(")
                    output.Add(top);
                else
                    throw new ArgumentException("Syntax error"); // A closing paren was not found
            }
            return output;
        }
        /// <summary>
        /// Returns the calculation result from an array of characters organized in Reverse Polish Notation.
        /// </summary>
        /// <param name="rpnStack">An array of characters organized in Reverse Polish Notation</param>
        public static int CalculateFromRPN(this List<string> rpnStack)
        {
            if (rpnStack.Count < 1)
                throw new ArgumentException("Syntax error"); // Possible that only parens were entered
            var resultStack = new Stack<int>();
            foreach (var token in rpnStack)
            {
                if (Int32.TryParse(token, out int parsed))
                    resultStack.Push(parsed);
                else if ((token == "+" || token == "–" || token == "*" || token == "/") && resultStack.Count > 1)
                    resultStack.Push(PerformRPNOperation(resultStack.Pop(), resultStack.Pop(), token));
                else
                    throw new ArgumentException("Syntax error"); // Too many operators entered
            }
            if (resultStack.Count == 1)
                return resultStack.Pop(); // Return the final value
            else
                throw new ArgumentException("Syntax error"); // Too few operators entered
        }
        /// <summary>
        /// Returns the result from the operation.
        /// </summary>
        /// <param name="b">The first item popped from the stack.</param>
        /// <param name="a">The second item popped from the stack.</param>
        /// <param name="op">The operation char (+–/*).</param>
        private static int PerformRPNOperation(int b, int a, string op)
        {
            switch (op) // Decimal methods are used for accuracy
            {
                case "+":
                    return a + b;
                case "–":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    return a / b;
                default:
                    throw new ArgumentException("Invalid operation character.");
            }
        }
        /// <summary>
        /// Returns a string array that contains every character in the original expression, but with contiguous numbers
        /// grouped as they are found.
        /// </summary>
        /// <param name="s">Mathematical expression</param>
        /// <returns></returns>
        private static string[] StrArrayFromCalcInput(string s)
        {
            var matches = Regex.Matches(s, @"(-)?\d+|\D"); // Groups any contiguous numbers including a possible –, or single chars
            var arr = new string[matches.Count];
            for (var i = 0; i < matches.Count; i++)
                arr[i] = matches[i].Groups[0].Value;
            return arr;
        }
    }
}
