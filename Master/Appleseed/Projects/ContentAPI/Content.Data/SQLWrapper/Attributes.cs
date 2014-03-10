////////////////////////////////////////////////////
// Attributes.cs
// Author: Andrey Shchurov, shchurov@gmail.com, 2005
////////////////////////////////////////////////////
#region using
using System;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

using Debug = System.Diagnostics.Debug;
#endregion

namespace Content.API.Data
{

	#region enums
	/// <summary>
	/// Command type enumeration for <see cref="SWCommandAttribute.CommandType"/> property
	/// of <see cref="SWCommandAttribute"/> attribute.
	/// </summary>
	public enum SWCommandType
	{
		/// <summary>
		/// Equals to <b>CommandType.Text</b> value. If <see cref="SWCommandAttribute"/> is defined 
		/// then it is a default value of <see cref="SWCommandAttribute.CommandType"/> property.
		/// </summary>
		Text,

		/// <summary>
		/// Equals to <b>CommandType.StoredProcedure</b> value. 
		/// </summary>
		StoredProcedure,

		/// <summary>
		/// The command implements insert/update operation for a table specified.
		/// </summary>
		InsertUpdate,

	}


	/// <summary>
	/// Parameter type enumeration for <see cref="SWParameterAttribute.ParameterType"/> property
	/// of <see cref="SWParameterAttribute"/> attribute.
	/// </summary>
	public enum SWParameterType
	{
		/// <summary>
		/// The parameter is defaul and has not special role.
		/// </summary>
		Default, 

		/// <summary>
		/// The parameter is a return value from a stored procedure.
		/// </summary>
		SPReturnValue, 

		/// <summary>
		/// This parameter is a part of a table key 
		/// and is applicable only 
		/// for <see cref="SWCommandType.InsertUpdate">SWCommandType.InsertUpdate</see> value 
		/// of method's <see cref="SWCommandAttribute.CommandType">SWCommandAttribute.CommandType</see> property.
		/// </summary>
		Key,
 
		/// <summary>
		/// This parameter is a part of a table autoincremental field 
		/// and is applicable only 
		/// for <see cref="SWCommandType.InsertUpdate">SWCommandType.InsertUpdate</see> value 
		/// of method's <see cref="SWCommandAttribute.CommandType">SWCommandAttribute.CommandType</see> property.
		/// This parameter requires to be only passed by refference.
		/// </summary>
		Identity,

	}
	#endregion

	#region SWCommandAttribute
	
	/// <summary>
	/// This attribute defines properties of generated methods.
	/// </summary>
	/// <remarks>
	/// If this attribute is not defined for a method to be generated then
	/// <see cref="System.Data.CommandType.StoredProcedure">CommandType.StoredProcedure</see> value is applied for underlying 
	/// <see cref="System.Data.SqlClient.SqlCommand"/> object.
	/// </remarks>
	[ AttributeUsage(AttributeTargets.Method) ]
	public sealed class SWCommandAttribute : Attribute
	{
		#region private members
		private SWCommandType m_commandType = SWCommandType.StoredProcedure;
		private string m_commandText = String.Empty;
		private object m_returnIfNull = null;
		private MissingSchemaAction m_missingSchemaAction = MissingSchemaAction.Add;

		private const string NullReturnValueToken = "I had nothing imagined but typing this silly string";
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SWCommandAttribute"/> class with the specified
		/// <see cref="SWCommandAttribute.CommandText"/> value
		/// </summary>
		/// <param name="commandText">Is a value of <see cref="SWCommandAttribute.CommandText"/> property</param>
		public SWCommandAttribute(string commandText) 
			: this(SWCommandType.Text, commandText, null, MissingSchemaAction.Add){}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="SWCommandAttribute"/> class with the specified
		/// <see cref="CommandText"/> and <see cref="ReturnIfNull"/> values
		/// </summary>
		/// <param name="commandText">Is a value of <see cref="SWCommandAttribute.CommandText"/> property</param>
		/// <param name="returnIfNull"></param>
		public SWCommandAttribute(string commandText, object returnIfNull) 
			: this(SWCommandType.Text, commandText, returnIfNull, MissingSchemaAction.Add){}

		/// <summary>
		/// Initializes a new instance of the <see cref="SWCommandAttribute"/> class with the specified
		/// <see cref="CommandType"/> and <see cref="CommandText"/> values
		/// </summary>
		/// <param name="commandType"></param>
		/// <param name="commandText">Is a value of <see cref="SWCommandAttribute.CommandText"/> property</param>
		public SWCommandAttribute(SWCommandType commandType, string commandText) 
			: this(commandType, commandText, null, MissingSchemaAction.Add){}

		/// <summary>
		/// Initializes a new instance of the <see cref="SWCommandAttribute"/> class with the specified
		/// <see cref="CommandType"/>, <see cref="CommandText"/>
		/// and <see cref="ReturnIfNull"/> values
		/// </summary>
		/// <param name="commandType">Is a value of <see cref="SWCommandAttribute.CommandType"/> property</param>
		/// <param name="commandText">Is a value of <see cref="SWCommandAttribute.CommandText"/> property</param>
		/// <param name="returnIfNull">Is a value of <see cref="SWCommandAttribute.ReturnIfNull"/> property</param>
		/// <param name="missingSchemaAction">Is a value of <see cref="SWCommandAttribute.MissingSchemaAction"/> property</param>
		public SWCommandAttribute(SWCommandType commandType, string commandText, object returnIfNull, MissingSchemaAction missingSchemaAction) 
		{
			m_commandType = commandType;
			m_commandText = commandText;
			m_returnIfNull = returnIfNull;
			if(m_returnIfNull is string) 
				if((string)m_returnIfNull == NullReturnValueToken) 
					m_returnIfNull = null;

			m_missingSchemaAction = missingSchemaAction;
		}

		/// <summary>
		/// An attribute builder method
		/// </summary>
		/// <param name="attr"></param>
		/// <returns></returns>
		public static CustomAttributeBuilder GetCustomAttributeBuilder(SWCommandAttribute attr)
		{
			string commandText = attr.m_commandText;
			object returnIfNull = attr.m_returnIfNull;
			if(returnIfNull == null) returnIfNull = NullReturnValueToken;
			Type[] arrParamTypes = new Type[] {typeof(SWCommandType), typeof(string), typeof(object), typeof(MissingSchemaAction)};
			object[] arrParamValues = new object[] {attr.m_commandType, commandText, returnIfNull, attr.m_missingSchemaAction};
			ConstructorInfo ctor = typeof(SWCommandAttribute).GetConstructor(arrParamTypes);
			return new CustomAttributeBuilder(ctor, arrParamValues);
		}
		#endregion

		#region Properties

		/// <summary>
		/// Defines the meaning of a CommandText property.
		/// </summary>
		/// <remarks>
		/// The meaning of a CommandText property depending of a CommendType property:
		/// <list type="table">
		/// <listheader>
		/// <term>CommandType value</term><description>CommandText meaning</description>
		/// </listheader>
		/// <item><term><see cref="SWCommandType.Text"/></term><description>An SQL expression</description></item>
		/// <item><term><see cref="SWCommandType.StoredProcedure"/> (default value)</term><description>A name of a stored procedure</description></item>
		/// <item><term><see cref="SWCommandType.InsertUpdate"/></term><description>A name of a table of a view</description></item>
		/// </list>
		/// If this property is not defined then <see cref="SWCommandType.StoredProcedure"/> as a default value.
		/// </remarks>
		public SWCommandType CommandType
		{
			get { return m_commandType; }
			set { m_commandType = value; }
		}

		/// <summary>
		/// This is a text of a command and is interpreted 
		/// according to a value of <see cref="CommandType"/> property.
		/// </summary>
		/// <remarks>
		/// The meaning of a CommandText property depending of a CommendType property:
		/// <list type="table">
		/// <listheader>
		/// <term>CommandType value</term><description>CommandText meaning</description>
		/// </listheader>
		/// <item><term><see cref="SWCommandType.Text"/></term><description>An SQL expression</description></item>
		/// <item><term><see cref="SWCommandType.StoredProcedure"/></term><description>A name of a stored procedure</description></item>
		/// <item><term><see cref="SWCommandType.InsertUpdate"/></term><description>A name of a table of a view</description></item>
		/// </list>
		/// If this value is not defined then the name of the method is used as a command text.
		/// </remarks>
		public string CommandText
		{
			get { return m_commandText == null ? string.Empty : m_commandText; }
			set { m_commandText = value; }
		}

		/// <summary>
		/// A value that will be returned if the command returns null. 
		/// This property should be defined if your generated method returns 
		/// a value type value and you expect that re result of the method execution 
		/// can be null. In this case the value of this property will be returned.
		/// </summary>
		public object ReturnIfNull
		{
			get { return m_returnIfNull; }
			set { m_returnIfNull = value; }
		}

		
		/// <summary>
		/// 
		/// </summary>
		public MissingSchemaAction MissingSchemaAction
		{
			get { return m_missingSchemaAction; }
			set { m_missingSchemaAction = value; }
		}
		#endregion

	}
    
	#endregion

	#region SWParameterAttribute

	/// <summary>
	/// This attribute defines properties of method's parameters
	/// </summary>
	[ AttributeUsage(AttributeTargets.Parameter) ]
	public class SWParameterAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		internal const string NullReturnValueToken = "Yet another silly string ls;dkfmv lskfmv sdkfmv sdkfv ";

		#region Private members
		private string m_name = "";
		private const SqlDbType ParamTypeNotDefinedValue = (SqlDbType)1000000;
		private SqlDbType m_sqlDbType = ParamTypeNotDefinedValue;
		private int m_size = 0;
		private byte m_precision = 0;
		private byte m_scale = 0;
		private object m_treatAsNull = NullReturnValueToken;
		private SWParameterType m_parameterType = SWParameterType.Default;
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SWParameterAttribute"/> class
		/// </summary>
		public SWParameterAttribute() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="SWParameterAttribute"/> class
		/// </summary>
		/// <param name="name">Is a value of <see cref="Name"/> property</param>
		public SWParameterAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SWParameterAttribute"/> class
		/// </summary>
		/// <param name="size">Is a value of <see cref="Size"/> property</param>
		public SWParameterAttribute(int size)
		{
			Size = size;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SWParameterAttribute"/> class
		/// </summary>
		/// <param name="parameterType">Is a value of <see cref="ParameterType"/> property</param>
		public SWParameterAttribute(SWParameterType parameterType)
		{
			ParameterType = parameterType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SWParameterAttribute"/> class
		/// </summary>
		/// <param name="parameterType">Is a value of <see cref="ParameterType"/> property</param>
		/// <param name="size">Is a value of <see cref="Size"/> property</param>
		public SWParameterAttribute(SWParameterType parameterType, int size)
		{
			ParameterType = parameterType;
			Size = size;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SWParameterAttribute"/> class
		/// </summary>
		/// <param name="parameterType">Is a value of <see cref="ParameterType"/> property</param>
		/// <param name="name">Is a value of <see cref="Name"/> property</param>
		public SWParameterAttribute(SWParameterType parameterType, string name)
		{
			ParameterType = parameterType;
			m_name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SWParameterAttribute"/> class
		/// </summary>
		/// <param name="name">Is a value of <see cref="Name"/> property</param>
		/// <param name="size">Is a value of <see cref="Size"/> property</param>
		public SWParameterAttribute(string name, int size)
		{
			Name = name;
			Size = size;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SWParameterAttribute"/> class with the specified
		/// <see cref="Name"/>, <see cref="SqlDbType"/>, <see cref="Size"/>, <see cref="Precision"/>, 
		/// <see cref="Scale"/>, <see cref="TreatAsNull"/> and <see cref="ParameterType"/> values
		/// </summary>
		/// <param name="name">Is a value of <see cref="Name"/> property</param>
		/// <param name="sqlDbType">Is a value of <see cref="SqlDbType"/> property</param>
		/// <param name="size">Is a value of <see cref="Size"/> property</param>
		/// <param name="precision">Is a value of <see cref="Precision"/> property</param>
		/// <param name="scale">Is a value of <see cref="Scale"/> property</param>
		/// <param name="treatAsNull">Is a value of <see cref="TreatAsNull"/> property</param>
		/// <param name="parameterType">Is a value of <see cref="ParameterType"/> property</param>
		public SWParameterAttribute(string name, SqlDbType sqlDbType, int size, byte precision, byte scale, object treatAsNull, SWParameterType parameterType)
		{
			Name = name;
			SqlDbType = sqlDbType;
			Size = size;
			Precision = precision;
			Scale = scale;
			TreatAsNull = treatAsNull;
			ParameterType = parameterType;
		}

		/// <summary>
		/// An attribute builder method
		/// </summary>
		/// <param name="attr"></param>
		/// <returns></returns>
		public static CustomAttributeBuilder GetCustomAttributeBuilder(SWParameterAttribute attr)
		{
			string name = attr.m_name;
			Type[] arrParamTypes = new Type[] {typeof(string), typeof(SqlDbType), typeof(int), typeof(byte), typeof(byte), typeof(object), typeof(SWParameterType)};
			object[] arrParamValues = new object[] {name, attr.m_sqlDbType, attr.m_size, attr.m_precision, attr.m_scale, attr.m_treatAsNull, attr.m_parameterType};
			ConstructorInfo ctor = typeof(SWParameterAttribute).GetConstructor(arrParamTypes);
			return new CustomAttributeBuilder(ctor, arrParamValues);
		}
		#endregion

		#region Properties

		/// <summary>
		/// Sql parameter name. If this property is not defined 
		/// then a method parameter name is used.
		/// </summary>
		public string Name
		{
			get { return m_name == null ? string.Empty : m_name; }
			set { m_name = value; }
		}

		/// <summary>
		/// Sql parameter size. 
		/// It is strongly recomended to define this property for string parameters
		/// so that they could be trimmed to the size specified.
		/// </summary>
		public int Size
		{
			get { return m_size; }
			set { m_size = value; }
		}

		/// <summary>
		/// Sql parameter precision. It has not sense for non-numeric parameters.
		/// </summary>
		public byte Precision
		{
			get { return m_precision; }
			set { m_precision = value; }
		}

		/// <summary>
		/// Sql parameter scale. It has not sense for non-numeric parameters.
		/// </summary>
		public byte Scale
		{
			get { return m_scale; }
			set { m_scale = value; }
		}

		/// <summary>
		/// This parameter contains a value that will be interpreted as null. 
		/// This is usefull if you want to pass a null to a value type parameter.
		/// </summary>
		public object TreatAsNull
		{
			get { return m_treatAsNull; }
			set { m_treatAsNull = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public SWParameterType ParameterType
		{
			get { return m_parameterType; }
			set { m_parameterType = value; }
		}

		/// <summary>
		/// Sql parameter type. 
		/// If not defined then method parameter type is converted to <see cref="SqlDbType"/> type
		/// </summary>
		public SqlDbType SqlDbType
		{
			get 
			{ 
				return m_sqlDbType; 
			}

			set 
			{ 
				m_sqlDbType = value; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		internal bool IsNameDefined
		{
			get { return m_name != null && m_name.Length != 0; }
		}

		/// <summary>
		/// 
		/// </summary>
		internal bool IsSizeDefined
		{
			get { return m_size > 0; }
		}

		/// <summary>
		/// 
		/// </summary>
		internal bool IsTypeDefined
		{
			get { return m_sqlDbType != ParamTypeNotDefinedValue; }
		}

		/// <summary>
		/// 
		/// </summary>
		internal bool IsScaleDefined
		{
			get { return m_scale > 0; }
		}

		/// <summary>
		/// 
		/// </summary>
		internal bool IsPrecisionDefined
		{
			get { return m_precision > 0; }
		}
		#endregion

	}

	#endregion

}

