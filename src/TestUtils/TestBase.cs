using System;
using System.Reflection;

namespace SIL.SpeechTools.TestUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Use to derive tests from when the tests need to access protected and private members
	/// of objects they're testing.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TestBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a string value returned from a call to a private method.
		/// </summary>
		/// <param name="binding">This is either the Type of the object on which the method
		/// is called or an instance of that type of object. When the method being called
		/// is static then binding should be a type.</param>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="args">An array of arguments to pass to the method call.</param>
		/// ------------------------------------------------------------------------------------
		protected string GetStrResult(object binding, string methodName, object[] args)
		{
			return (GetResult(binding, methodName, args) as string);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a string value returned from a call to a private method.
		/// </summary>
		/// <param name="binding">This is either the Type of the object on which the method
		/// is called or an instance of that type of object. When the method being called
		/// is static then binding should be a type.</param>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="args">An single arguments to pass to the method call.</param>
		/// ------------------------------------------------------------------------------------
		protected string GetStrResult(object binding, string methodName, object args)
		{
			return (GetResult(binding, methodName, args) as string);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a integer value returned from a call to a private method.
		/// </summary>
		/// <param name="binding">This is either the Type of the object on which the method
		/// is called or an instance of that type of object. When the method being called
		/// is static then binding should be a type.</param>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="args">An single arguments to pass to the method call.</param>
		/// ------------------------------------------------------------------------------------
		protected int GetIntResult(object binding, string methodName, object args)
		{
			return ((int)GetResult(binding, methodName, args));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a integer value returned from a call to a private method.
		/// </summary>
		/// <param name="binding">This is either the Type of the object on which the method
		/// is called or an instance of that type of object. When the method being called
		/// is static then binding should be a type.</param>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="args">An array of arguments to pass to the method call.</param>
		/// ------------------------------------------------------------------------------------
		protected int GetIntResult(object binding, string methodName, object[] args)
		{
			return ((int)GetResult(binding, methodName, args));
		}

  	  	/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a float value returned from a call to a private method.
		/// </summary>
		/// <param name="binding">This is either the Type of the object on which the method
		/// is called or an instance of that type of object. When the method being called
		/// is static then binding should be a type.</param>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="args">An single arguments to pass to the method call.</param>
		/// ------------------------------------------------------------------------------------
		protected float GetFloatResult(object binding, string methodName, object args)
		{
			return ((float)GetResult(binding, methodName, args));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a float value returned from a call to a private method.
		/// </summary>
		/// <param name="binding">This is either the Type of the object on which the method
		/// is called or an instance of that type of object. When the method being called
		/// is static then binding should be a type.</param>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="args">An array of arguments to pass to the method call.</param>
		/// ------------------------------------------------------------------------------------
		protected float GetFloatResult(object binding, string methodName, object[] args)
		{
			return ((float)GetResult(binding, methodName, args));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a boolean value returned from a call to a private method.
		/// </summary>
		/// <param name="binding">This is either the Type of the object on which the method
		/// is called or an instance of that type of object. When the method being called
		/// is static then binding should be a type.</param>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="args">An single arguments to pass to the method call.</param>
		/// ------------------------------------------------------------------------------------
		protected bool GetBoolResult(object binding, string methodName, object args)
		{
			return ((bool)GetResult(binding, methodName, args));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a boolean value returned from a call to a private method.
		/// </summary>
		/// <param name="binding">This is either the Type of the object on which the method
		/// is called or an instance of that type of object. When the method being called
		/// is static then binding should be a type.</param>
		/// <param name="methodName">Name of the method to call.</param>
		/// <param name="args">An array of arguments to pass to the method call.</param>
		/// ------------------------------------------------------------------------------------
		protected bool GetBoolResult(object binding, string methodName, object[] args)
		{
			return ((bool)GetResult(binding, methodName, args));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calls a method specified on the specified binding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CallMethod(object binding, string methodName, object[] args)
		{
			GetResult(binding, methodName, args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calls a method specified on the specified binding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void CallMethod(object binding, string methodName, object args)
		{
			GetResult(binding, methodName, args);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the result of calling a method on the specified binding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public object GetResult(object binding, string methodName, object args)
		{
			return Invoke(binding, methodName, new object[] {args}, BindingFlags.InvokeMethod);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the result of calling a method on the specified binding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public object GetResult(object binding, string methodName, object[] args)
		{
			return Invoke(binding, methodName, args, BindingFlags.InvokeMethod);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the specified property on the specified binding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void SetProperty(object binding, string propertyName, object args)
		{
			Invoke(binding, propertyName, new object[] {args}, BindingFlags.SetProperty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the specified field (i.e. member variable) on the specified binding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void SetField(object binding, string fieldName, object args)
		{
			Invoke(binding, fieldName, new object[] {args}, BindingFlags.SetField);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the specified property on the specified binding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected object GetProperty(object binding, string propertyName)
		{
			return Invoke(binding, propertyName, null, BindingFlags.GetProperty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the specified field (i.e. member variable) on the specified binding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected object GetField(object binding, string fieldName)
		{
			return Invoke(binding, fieldName, null, BindingFlags.GetField);
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the specified member variable or property (specified by name) on the
		/// specified binding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private object Invoke(object binding, string name, object[] args, BindingFlags flags)
		{
			flags |= BindingFlags.NonPublic;

			// If binding is a Type then assume invoke on a static method, property or field.
			// Otherwise invoke on an instance method, property or field.
			if (binding is Type)
			{
				return ((binding as Type).InvokeMember(name,
					flags | BindingFlags.Static, null, binding, args));
			}
			else
			{
				return binding.GetType().InvokeMember(name,
					flags | BindingFlags.Instance, null, binding, args);
			}
		}
	}
}
