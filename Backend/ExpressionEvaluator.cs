using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace Backend;

public static class ExpressionEvaluator
{
    public static double Evalute(string infix) => EvalutePostfix(ToPostfix(infix));

    private static string ToPostfix(string infix)
    {
        var posfix = string.Empty;
        var stack = new Stack<char>();
        int i = 0;
        //foreach (var item in infix)
        while (i < infix.Length)
        {
            var item = infix[i];

            if (IsOperator(item))
            {
                if (item == ')')
                {
                    var ope = stack.Pop();
                    while (ope != '(')
                    {
                        //posfix += ope;

                        posfix += " " + ope;
                        ope = stack.Pop();
                    }
                }
                else
                {
                    if (stack.Count == 0)
                    {
                        stack.Push(item);
                    }
                    else
                    {
                        if (PriorityInfix(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item);
                        }
                        else
                        {
                            //posfix += stack.Pop();
                            posfix += " " + stack.Pop();
                            stack.Push(item);
                        }
                    }
                }
                i++;
            }
            else if (char.IsDigit(item))
            {
                string number = string.Empty;
                while (i < infix.Length && char.IsDigit(infix[i]))
                {
                    number += infix[i];
                    i++;
                }
                posfix += " " + number;
            }
            else
            {
                i++;
            }
        }
        while (stack.Count != 0)
        {
            posfix += " " + stack.Pop();
        }
        return posfix.Trim();
    }
    private static int PriorityStack(char op) => op switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Invalid expression."),
    };

    private static int PriorityInfix(char op) => op switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Invalid expression."),
    };

    private static bool IsOperator(char item) => item == '^' || item == '*' || item == '/' || item == '+' || item == '-' || item == '(' || item == ')';

    private static double EvalutePostfix(string postfix)
    {
        var stack = new Stack<double>();
        var tokens = postfix.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        //foreach (var item in postfix)
          foreach (var token in tokens)
            {
            if (token.Length == 1 && IsOperator(token[0]))
            {
                var ope2 = stack.Pop();
                var ope1 = stack.Pop();
                stack.Push(Calculate(ope1, ope2, token[0]));
            }
            else
            {
                //stack.Push(char.GetNumericValue(item));
                  stack.Push(double.Parse(token));
            }
        }
        return stack.Pop();
    }

    private static double Calculate(double ope1, double ope2, char item) => item switch
    {
        '*' => ope1 * ope2,
        '/' => ope1 / ope2,
        '+' => ope1 + ope2,
        '-' => ope1 - ope2,
        '^' => Math.Pow(ope1, ope2),
        _ => throw new Exception("Invalid expression."),
    };
}
