using System;
using System.IO;

namespace Lumina.Text.Expressions;

/// <summary>
/// Represent an Expression containing an operator with two arguments.
/// </summary>
public class BinaryExpression : BaseExpression
{
    /// <summary>
    /// Construct a new UnaryExpression with given type and operand.
    /// </summary>
    public BinaryExpression( ExpressionType typeByte, BaseExpression operand1, BaseExpression operand2 )
    {
        if( !IsBinaryType( typeByte ) )
            throw new ArgumentException( $"Given value does not indicate an {nameof( ParameterExpression )}.", nameof( typeByte ) );
        ExpressionType = typeByte;
        Operand1 = operand1;
        Operand2 = operand2;
    }

    /// <summary>
    /// The first operand.
    /// </summary>
    public BaseExpression Operand1 { get; }

    /// <summary>
    /// The second operand.
    /// </summary>
    public BaseExpression Operand2 { get; }

    /// <inheritdoc />
    public override int Size => 1 + Operand1.Size + Operand2.Size;

    /// <inheritdoc />
    public override ExpressionType ExpressionType { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return ExpressionType switch
        {
            ExpressionType.GreaterThanOrEqualTo => $"BinExpr{{{Operand1} >= {Operand2}}}",
            ExpressionType.GreaterThan => $"BinExpr{{{Operand1} > {Operand2}}}",
            ExpressionType.LessThanOrEqualTo => $"BinExpr{{{Operand1} <= {Operand2}}}",
            ExpressionType.LessThan => $"BinExpr{{{Operand1} < {Operand2}}}",
            ExpressionType.Equal => $"BinExpr{{{Operand1} == {Operand2}}}",
            ExpressionType.NotEqual => $"BinExpr{{{Operand1} != {Operand2}}}",
            _ => throw new NotImplementedException() // cannot reach, as this instance is immutable and this field is filtered from constructor
        };
    }

    /// <summary>
    /// Identify whether the given type byte indicates an UnaryExpression.
    /// </summary>
    /// <param name="typeByte">Type byte to identify.</param>
    /// <returns>True if it indicates an UnaryExpression.</returns>
    public static bool IsBinaryType( ExpressionType typeByte )
    {
        return (byte)typeByte switch
        {
            >= 0xE0 and <= 0xE5 => true,
            _ => false
        };
    }

    /// <inheritdoc />
    public override void Encode( Stream stream )
    {
        stream.WriteByte( (byte)ExpressionType );
        Operand1.Encode( stream );
        Operand2.Encode( stream );
    }

    /// <summary>
    /// Parse given Stream into a BinaryExpression.
    /// </summary>
    /// <param name="typeByte">Type marker byte.</param>
    /// <param name="stream">Stream to read from.</param>
    /// <returns>Parsed BinaryExpression.</returns>
    public static BinaryExpression Parse( byte typeByte, Stream stream )
    {
        var operand1 = BaseExpression.Parse( stream );
        var operand2 = BaseExpression.Parse( stream );
        return new BinaryExpression( (ExpressionType)typeByte, operand1, operand2 );
    }
}