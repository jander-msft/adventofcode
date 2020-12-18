using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static AOC2020.Day18;

namespace AOC2020
{
    public class Day18 : BaseDay<Node>
    {
        public Day18() : base("Day18", "31142189909908", "323912478287549")
        {
        }

        protected override Node Parse(StreamReader reader)
        {
            static Node ParseSpan(ReadOnlySpan<char> span)
            {
                int index = -1;
                Node rightNode = null;

                for (int i = span.Length - 1; i >= 0; i--)
                {
                    char c = span[i];

                    if (!char.IsWhiteSpace(c))
                    {
                        if (char.IsDigit(c))
                        {
                            int end = i;
                            do
                            {
                                i--;
                                if (i < 0)
                                {
                                    break;
                                }
                                c = span[i];
                            }
                            while (char.IsDigit(c));

                            index = i;
                            i++;

                            rightNode = new ValueNode(Int64.Parse(span.Slice(end, end - i + 1)));
                            break;
                        }
                        else if (c == ')')
                        {
                            int openCount = 1;
                            int end = i - 1;
                            do
                            {
                                i--;
                                if (i < 0)
                                {
                                    break;
                                }
                                c = span[i];
                                if (c == ')')
                                {
                                    openCount++;
                                }
                                else if (c == '(')
                                {
                                    openCount--;
                                }
                            }
                            while (c != '(' || openCount != 0);

                            if (i < 0)
                            {
                                index = i;
                            }
                            else
                            {
                                Debug.Assert(c == '(');
                                index = i - 1;
                            }
                            i++;

                            rightNode = new GroupNode(ParseSpan(span.Slice(i, end - i + 1)));
                            break;
                        }
                    }
                }

                if (index < 0)
                {
                    Debug.Assert(null != rightNode);
                    return rightNode;
                }

                Operator op = Operator.Add;
                for (int i = index; i >= 0; i--)
                {
                    char c = span[i];
                    if (!char.IsWhiteSpace(c))
                    {
                        op = ToOperator(c);
                        index = i;
                        break;
                    }
                }

                Node leftNode = ParseSpan(span.Slice(0, index));

                return new BinaryNode(op, leftNode, rightNode);
            }

            return ParseSpan(reader.ReadLine());
        }

        protected override string Solve1(Node[] items)
        {
            return items.Sum(i => i.Evaluate()).ToString();
        }

        protected override string Solve2(Node[] items)
        {
            static Node Rebalance(Node root)
            {
                if (root is ValueNode valueNode)
                {
                    return new ValueNode(valueNode.Value);
                }
                else if (root is GroupNode groupNode)
                {
                    return new GroupNode(Rebalance(groupNode.Inner));
                }
                else if (root is BinaryNode binaryNode)
                {
                    Operator op = binaryNode.Operator;
                    Node left = Rebalance(binaryNode.Left);
                    Node right = Rebalance(binaryNode.Right);

                    // Lower nodes (closer to leaves) in the tree have more precedence over
                    // higher nodes (closer to the root). Part2 requires that + takes precedence
                    // over *, thus if a * is an immediate child of a + node, then rewrite to
                    // respect the precendence. We can ignore checking the right node from the
                    // original AST because it is always a Value or a Group, neither of which can
                    // be broken apart for rebalancing. Thus, only look for something that looks
                    // like: a * b + c which is represented as <a * b> + c in the AST. This needs
                    // to be rebalanced to a * <b + c> in order to have the evaluator not have to
                    // think about precendence between nodes.
                    if (op == Operator.Add &&
                        left is BinaryNode leftBinaryNode &&
                        leftBinaryNode.Operator == Operator.Multiply)
                    {
                        op = Operator.Multiply;

                        left = leftBinaryNode.Left;

                        right = new BinaryNode(Operator.Add, leftBinaryNode.Right, right);
                    }

                    return new BinaryNode(op, left, right);
                }
                throw new NotSupportedException();
            }

            // Rewrite AST so that + takes precendence over *
            return items.Select(Rebalance).Sum(i => i.Evaluate()).ToString();
        }

        public static string ToSymbol(Operator op)
        {
            switch (op)
            {
                case Operator.Add:
                    return "+";
                case Operator.Multiply:
                    return "*";
            }
            throw new NotSupportedException();
        }

        public static Operator ToOperator(char symbol)
        {
            switch (symbol)
            {
                case '+':
                    return Operator.Add;
                case '*':
                    return Operator.Multiply;
            }
            throw new NotSupportedException();
        }

        [DebuggerDisplay("{DebuggerDisplay}")]
        public abstract class Node
        {
            internal abstract string DebuggerDisplay { get; }

            public abstract long Evaluate();
        }

        public class ValueNode : Node
        {
            public ValueNode(long value)
            {
                Value = value;
            }

            public long Value { get; }

            internal override string DebuggerDisplay => Value.ToString();

            public override long Evaluate()
            {
                return Value;
            }
        }

        public class BinaryNode : Node
        {
            public BinaryNode(Operator op, Node left, Node right)
            {
                Operator = op;
                Left = left;
                Right = right;
            }

            public Operator Operator { get; }

            public Node Left { get; }

            public Node Right { get; }

            internal override string DebuggerDisplay => string.Concat(
                Left.DebuggerDisplay, "", ToSymbol(Operator), "", Right.DebuggerDisplay);

            public override long Evaluate()
            {
                long left = Left.Evaluate();
                long right = Right.Evaluate();
                switch (Operator)
                {
                    case Operator.Add:
                        return left + right;
                    case Operator.Multiply:
                        return left * right;
                }
                throw new NotSupportedException();
            }
        }

        public class GroupNode : Node
        {
            public GroupNode(Node inner)
            {
                Inner = inner;
            }

            public Node Inner { get; }

            internal override string DebuggerDisplay => string.Concat(
                "(", Inner.DebuggerDisplay, ")");

            public override long Evaluate()
            {
                return Inner.Evaluate();
            }
        }

        public enum Operator
        {
            Add,
            Multiply
        }
    }
}
