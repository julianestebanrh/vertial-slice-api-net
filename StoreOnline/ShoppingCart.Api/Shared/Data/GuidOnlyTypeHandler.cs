using System.Data;
using static Dapper.SqlMapper;

namespace ShoppingCart.Api.Shared.Data
{
	internal sealed class GuidOnlyTypeHandler : TypeHandler<Guid>
	{
		public override Guid Parse(object value) => new Guid((string)value);

		public override void SetValue(IDbDataParameter parameter, Guid value)
		{
			parameter.Value = value.ToString();
		}
	}
}
