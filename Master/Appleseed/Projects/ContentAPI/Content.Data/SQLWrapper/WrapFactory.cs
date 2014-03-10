////////////////////////////////////////////////////
// WrapFactory.cs
// Author: Andrey Shchurov, shchurov@gmail.com, 2005
////////////////////////////////////////////////////
#region using
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Data;
using System.Data.Common;
using System.Collections;
#endregion

namespace Content.API.Data
{

	/// <summary>
	/// This class contains a static method <see cref="Create"/> 
	/// which generates class implementation from class 
	/// derived from <see cref="ISqlWrapperBase"/> imterface.
	/// </summary>
	public sealed class WrapFactory
	{
		/// <summary>
		/// ModuleBuilder object.
		/// </summary>
		private static ModuleBuilder m_modBuilder = null;

		/// <summary>
		/// AssemblyBuilder object.
		/// </summary>
		private static AssemblyBuilder m_asmBuilder = null;

		/// <summary>
		/// A static constructor.
		/// </summary>
		static WrapFactory()
		{
			AssemblyName asmName = new AssemblyName();
			asmName.Name = "SqlWrapperDynamicAsm";
			m_asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
			m_modBuilder = m_asmBuilder.DefineDynamicModule("SqlWrapperDynamicModule");
		}

		/// <summary>
		/// A private constructor designed in order that no one could create 
		/// an instance of this class.
		/// </summary>
		private WrapFactory()
		{
		}
		

		/// <summary>
		/// Create the class implementation of a class 
		/// derived from <see cref="ISqlWrapperBase"/> imterface.
		/// </summary>
		/// <param name="typeToWrap">A class type</param>
		/// <returns></returns>
		public static SqlWrapperBase Create(Type typeToWrap)
		{
			// Verifying base interface
			Type baseInterfaceType = typeof(ISqlWrapperBase);
			Type baseitf = typeToWrap.GetInterface(baseInterfaceType.FullName);
//			if(!typeToWrap.IsSubclassOf(typeof(SqlWrapperBase))){}
			if (baseitf != baseInterfaceType) 
			{
				string msg = string.Format ("interface {0} must inherit from base interface {1}", typeToWrap.FullName, baseInterfaceType.FullName);
				throw new SqlWrapperException(msg);
			}

			Type typeCreated = null;
			lock(typeof(WrapFactory))
			{
				// get a generated class name
				string strTypeName = "_$Impl" + typeToWrap.FullName;
				
				// try to find the class among already generated classes
				typeCreated = m_modBuilder.GetType(strTypeName);

				if(typeCreated == null)
				{
					TypeBuilder tb = null;
					if(IsInterface(typeToWrap))
					{
						// if the original type is an interface then create a new type derived from SqlWrapperBase class
						tb = m_modBuilder.DefineType(strTypeName, TypeAttributes.Class | TypeAttributes.Public, typeof(SqlWrapperBase));
					}
					else
					{
						// else create a new type derived from original type
						tb = m_modBuilder.DefineType(strTypeName, TypeAttributes.Class | TypeAttributes.Public, typeToWrap);
					}

					// add created type implementation
					AddTypeImplementation(tb, typeToWrap);

					// get created type
					typeCreated = tb.CreateType();
				}
			}

			// creat an instance of the created type
			SqlWrapperBase target = (SqlWrapperBase)Activator.CreateInstance(typeCreated);

			return target;
		}


		/// <summary>
		/// Adds an implementation to the created type.
		/// </summary>
		/// <param name="tb">Type builder object.</param>
		/// <param name="typeToWrap">Created class.</param>
		private static void AddTypeImplementation(TypeBuilder tb, Type typeToWrap) 
		{
			// add implementation for the interface
			if(IsInterface(typeToWrap))
			{
				tb.AddInterfaceImplementation(typeToWrap);
			}

			// create implementation for each type abstract method
			MethodInfo[] parentMethodInfo = typeToWrap.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |BindingFlags.Instance /* | BindingFlags.DeclaredOnly */);
			for(int i = 0; i < parentMethodInfo.Length; ++i)
			{
				if(IsAbstractMethod(parentMethodInfo[i]))
				{
					AddMethodImplementation(parentMethodInfo[i], tb);
				}
			}
		}

		/// <summary>
		/// Adds attributes and an implementation to a method of the created class.
		/// </summary>
		/// <param name="method">MethodInfo object.</param>
		/// <param name="tb">Type builder object.</param>
		private static void AddMethodImplementation(MethodInfo method, TypeBuilder tb) 
		{
			// get method return type
			Type RetType = method.ReturnType;
		
			// get method paramete information
			ParameterInfo [] prms = method.GetParameters();
			
			int paramCount = prms.Length;

			// get method parameter types and names
			Type [] paramTypes = new Type [paramCount];
			string[] paramNames = new string[paramCount];
			ParameterAttributes[] paramAttrs = new ParameterAttributes[paramCount];
			for(int i = 0; i < paramCount; ++i) 
			{
				paramTypes[i] = prms[i].ParameterType;
				paramNames[i] = prms[i].Name;
				paramAttrs[i] = prms[i].Attributes;
			}

			// define method body
			MethodBuilder methodBody = tb.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.Virtual, method.ReturnType, paramTypes);

			// define method attribute if exists
			SWCommandAttribute CommandMethodAttr = (SWCommandAttribute)Attribute.GetCustomAttribute(method, typeof(SWCommandAttribute));
			if(CommandMethodAttr != null)
			{
				methodBody.SetCustomAttribute(SWCommandAttribute.GetCustomAttributeBuilder(CommandMethodAttr));
			}

			// define method parameters with their attributes
			for(int i = 0; i < paramCount; ++i) 
			{
				ParameterBuilder param = methodBody.DefineParameter(i+1, paramAttrs[i], paramNames[i]);

				SWParameterAttribute[] ParameterAttr = (SWParameterAttribute[])prms[i].GetCustomAttributes(typeof(SWParameterAttribute), false);

				if(ParameterAttr.Length > 0) param.SetCustomAttribute(SWParameterAttribute.GetCustomAttributeBuilder(ParameterAttr[0]));
			}

			// generate method body
			GenerateMethodBody(methodBody.GetILGenerator(), RetType, prms);

		}
		
		/// <summary>
		/// Generates an implementation to the method.
		/// </summary>
		/// <param name="ilGen">ILGenerator object.</param>
		/// <param name="RetType">Method's return type</param>
		/// <param name="prms">Method's parameters.</param>
		private static void GenerateMethodBody(ILGenerator ilGen, Type RetType, ParameterInfo [] prms) 
		{
			/*
			 * // declaring local variables
			 * MethodInfo V_0;
			 * Type V_1;
			 * object[] V_2;
			 * object V_3;
			 * RetType V_4;
			 * object[] V_5;
			 */
			LocalBuilder V_0 = ilGen.DeclareLocal(typeof (MethodInfo)); 
			LocalBuilder V_1 = ilGen.DeclareLocal(typeof (Type));
			LocalBuilder V_2 = ilGen.DeclareLocal(typeof (object[])); 
			LocalBuilder V_3 = ilGen.DeclareLocal(typeof (object));
			LocalBuilder V_4 = ilGen.DeclareLocal(RetType); 
			LocalBuilder V_5 = ilGen.DeclareLocal(typeof (object[])); 

			/*
			 * V_0 = MethodBase.GetCurrentMethod();
			 */
			MethodInfo miLocalMethod = typeof(MethodBase).GetMethod ("GetCurrentMethod");
			ilGen.Emit(OpCodes.Callvirt, miLocalMethod);
			ilGen.Emit(OpCodes.Castclass, typeof(MethodInfo));
			ilGen.Emit(OpCodes.Stloc_S, V_0);


			/*
			 * V_5 = new object[prms.Length];
			 */
			ilGen.Emit(OpCodes.Ldc_I4, prms.Length);
			ilGen.Emit(OpCodes.Newarr, typeof(object));
			ilGen.Emit(OpCodes.Stloc_S, V_5);

			for(int i = 0; i < prms.Length; ++i)
			{
				/*
				 * V_5[i] = {method argument in position i+1};
				 */
				ilGen.Emit(OpCodes.Ldloc_S, V_5);
				ilGen.Emit(OpCodes.Ldc_I4, i);
				ilGen.Emit(OpCodes.Ldarg_S, i+1);

				// if method argument is a value type then box it
				if(prms[i].ParameterType.IsValueType)
				{
					ilGen.Emit(OpCodes.Box, prms[i].ParameterType);
				}
				else if(prms[i].ParameterType.IsByRef)
				{
					// get type from reference type
					Type reftype = SWExecutor.GetRefType(prms[i].ParameterType);
					// and box by this type
					if(reftype != null)
					{
						ilGen.Emit(OpCodes.Ldobj, reftype);
						if(reftype.IsValueType)
						{
							ilGen.Emit(OpCodes.Box, reftype);
						}
					}
				}
				ilGen.Emit(OpCodes.Stelem_Ref);
			}

			/*
			 * V_5 = V_2;
			 */
			ilGen.Emit(OpCodes.Ldloc_S, V_5);
			ilGen.Emit(OpCodes.Stloc_S, V_2);

			/*
			 * V_3 = SWExecutor.ExecuteMethodAndGetResult(
			 *													m_connection, 
			 *													m_transaction, 
			 *													V_0, 
			 *													V_2
			 *													);
			 */
			// load this.m_connection
			ilGen.Emit(OpCodes.Ldarg_0);// this
			FieldInfo fldConnection = typeof(SqlWrapperBase).GetField("m_connection", BindingFlags.Instance|BindingFlags.NonPublic);
			ilGen.Emit(OpCodes.Ldfld, fldConnection);
			// load this.m_transaction
			ilGen.Emit(OpCodes.Ldarg_0); // this
			FieldInfo fldTransaction = typeof(SqlWrapperBase).GetField("m_transaction", BindingFlags.Instance|BindingFlags.NonPublic);
			ilGen.Emit(OpCodes.Ldfld, fldTransaction);
			// load V_0
			ilGen.Emit(OpCodes.Ldloc_S, V_0);
			// load V_2
			ilGen.Emit(OpCodes.Ldloc_S, V_2);
			// load this.m_autoCloseConnection
			ilGen.Emit(OpCodes.Ldarg_0); // this
			FieldInfo fldAutoCloseConnection = typeof(SqlWrapperBase).GetField("m_autoCloseConnection", BindingFlags.Instance|BindingFlags.NonPublic);
			ilGen.Emit(OpCodes.Ldfld, fldAutoCloseConnection);

			// call SWExecutor.ExecuteMethodAndGetResult methos
			MethodInfo miExecuteMethodAndGetResult = typeof (SWExecutor).GetMethod ("ExecuteMethodAndGetResult", BindingFlags.Public | BindingFlags.Static);
			ilGen.Emit(OpCodes.Call, miExecuteMethodAndGetResult);
			// result -> V_3
			ilGen.Emit (OpCodes.Stloc_S, V_3);


			 // returning parameters passed by reference
			for(int i = 0; i < prms.Length; ++i)
			{
				if(prms[i].ParameterType.IsByRef)
				{
					Type reftype = SWExecutor.GetRefType(prms[i].ParameterType);
					if(reftype != null)
					{
						/*
						 * {method argument in position i+1} = V_2[i]
						 */
						ilGen.Emit(OpCodes.Ldarg_S, i+1);
						ilGen.Emit(OpCodes.Ldloc_S, V_2);
						ilGen.Emit(OpCodes.Ldc_I4, i);
						ilGen.Emit(OpCodes.Ldelem_Ref);
						if(reftype.IsValueType)
						{
							ilGen.Emit(OpCodes.Unbox, reftype);
							ilGen.Emit(OpCodes.Ldobj, reftype);
						}
						ilGen.Emit(OpCodes.Stobj, reftype);
					}
				}
			}
			

			if(RetType.FullName != "System.Void")
			{
				/*
				 * return (RetType)V_3;
				 */
				ilGen.Emit(OpCodes.Ldloc_S, V_3);
				if(RetType.IsValueType)
				{
					ilGen.Emit(OpCodes.Unbox, RetType);
					ilGen.Emit(OpCodes.Ldobj, RetType);
				}
				else
				{
					ilGen.Emit(OpCodes.Castclass, RetType);
				}
				ilGen.Emit (OpCodes.Stloc_S, V_4);
				ilGen.Emit(OpCodes.Ldloc_S, V_4);
			}
			ilGen.Emit(OpCodes.Ret);

		}

		/// <summary>
		/// Checks if a type is an interface.
		/// </summary>
		/// <param name="typeToWrap">Type object.</param>
		/// <returns></returns>
		private static bool IsInterface(Type typeToWrap)
		{
			return typeToWrap.IsInterface;
		}

		/// <summary>
		/// Checks if a method is abstract.
		/// </summary>
		/// <param name="method">MethodInfo object.</param>
		/// <returns></returns>
		private static bool IsAbstractMethod(MethodInfo method)
		{
			return method.IsAbstract;
		}


		/// <summary>
		/// Saves the created assembly to a file.
		/// </summary>
		/// <param name="fileName">A file name.</param>
		public static void SaveAssemply(string fileName)
		{
			m_asmBuilder.Save(fileName);
		}
	}
}
