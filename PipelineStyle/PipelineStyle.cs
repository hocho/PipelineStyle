// Copyright (c) Code Superior, Inc. All rights reserved. 

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CodeSuperior.PipelineStyle
{
	/// <summary>
	/// Simple wrappers to turn actions to functions and other glue code used to create expressions for pipelining.      
	/// 
	/// The main methods here are 'Do' and 'To'
	/// Use 'Do' extensions to pass in an object, perform an action and return the same object
	/// Use 'To' extensions to pass in an object, perform a function and return another object.
	/// 
	/// Extension methods are 
	/// Do
	/// DoIf
	/// DoNotNull
	/// DoIsNull
	/// DoNotDefault
	/// DoIsDefault
	/// 
	/// To
	/// ToIf
	/// ToNotNull
	/// ToIsNull
	/// ToNotDefault
	/// ToIsDefault
	/// 
	/// For optimization reasons, a distinction is made between Null and Default. Default is used for primitive types,
	/// although it can be used for both reference, value and primitive types.
	/// 
	/// </summary>
	[DebuggerNonUserCode]		// Comment out this line if you want to step into the pipeline code
	static 
	public 
	class FunUtil
	{
		// -------------------------------------------------------------------------------------------------------------
		// Helper method

		public 
		static 
		bool
		IsDefault<T>(
			this T								obj)
		{
			return	EqualityComparer<T>.Default.Equals(obj, default(T));
		}

		// -------------------------------------------------------------------------------------------------------------
		// -------------------------------------------------------------------------------------------------------------
		// Do Section
		// -------------------------------------------------------------------------------------------------------------
		// -------------------------------------------------------------------------------------------------------------
		// 'Do' takes an object, performs an action, and returns the original object

		/// <summary>
		/// Performs an action on the given object.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="action">Action to invoke.</param>
		/// <returns>The object passed in.</returns>
		public 
		static
		T
		Do<T>(
			this T								obj,
			Action<T>							action)
		{
			action(obj);

			return obj;
		}

		// -------------------------------------------------------------------------------------------------------------
		// Do - If

		/// <summary>
		/// Performs an action on the given object, if isTrue is true.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="isTrue">Boolean value to determine if the action should be invoked.</param>
		/// <param name="action">Action to invoke.</param>
		/// <returns>The object passed in.</returns>
		public 
		static
		T
		DoIf<T>(
			this T								obj,
			bool								isTrue,
			Action<T>							action)
		{
			if (isTrue)
				action(obj);

			return obj;
		}

		/// <summary>
		/// Performs an action on the given object, if the predicate isTrue returns true.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="isTrue">Predicate to which the object is passed to determine if the action should be invoked.</param>
		/// <param name="action">Action to invoke.</param>
		/// <returns>The object passed in.</returns>
		public 
		static
		T
		DoIf<T>(
			this T								obj,
			Func<T, bool>						isTrue,
			Action<T>							action)
		{
			if (isTrue(obj))
				action(obj);

			return obj;
		}

		public 
		static
		T
		DoIf<T>(
			this T								obj,
			Func<T, bool>						isTrue,
			Action<T>							onTrue,
			Action<T>							onFalse)
		{
			if (isTrue(obj))
				onTrue(obj);
			else 
				onFalse(obj);

			return obj;
		}

		// -------------------------------------------------------------------------------------------------------------
		// Do - Null

		/// <summary>
		/// Performs an action on the given object, if the object is not null.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNotNull">Action to invoke.</param>
		/// <returns>The object passed in.</returns>
		public 
		static 
		T
		DoNotNull<T>(
			this T							obj,
			Action<T>						onNotNull)
		where 
			T								:	class 
		{
			if (obj != null)
				onNotNull(obj);

			return obj;
		}

		/// <summary>
		/// Performs an action on the given object, if the object is null.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNull">Action to invoke.</param>
		/// <returns>The object passed in.</returns>
		public 
		static 
		T
		DoIsNull<T>(
			this T								obj,
			Action								onNull)
		where 
			T								:	class 
		{
			if (obj == null)
				onNull();

			return obj;
		}

		// -------------------------------------------------------------------------------------------------------------
		// Do - Default
		
		/// <summary>
		/// Performs an action on the given object, if the object is not the default value.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNotDefault">Action to invoke.</param>
		/// <returns>The object passed in.</returns>
		public 
		static 
		T
		DoNotDefault<T>(
			this T								obj,
			Action<T>							onNotDefault)
		{
			if (!obj.IsDefault())
				onNotDefault(obj);

			return obj;
		}

		/// <summary>
		/// Performs an action on the given object, if the object is the default value.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNull">Action to invoke.</param>
		/// <returns>The object passed in.</returns>
		public 
		static 
		T
		DoIsDefault<T>(
			this T								obj,
			Action								onDefault)
		{
			if (obj.IsDefault())
				onDefault();

			return obj;
		}

		// -------------------------------------------------------------------------------------------------------------
		// If - Do

		/// <summary>
		/// Performs an action, if the given Boolean is true.
		/// </summary>
		/// <param name="isTrue">The Boolean object made fluent using PipelineStyle.</param>
		/// <param name="onTrue">Action to invoke.</param>
		/// <returns>The Boolean object passed in.</returns>
		public 
		static 
		bool
		IfDo(
			this bool							isTrue,
			Action								onTrue)
		{
			if (isTrue)
				onTrue();

			return isTrue;
		}

		/// <summary>
		/// Performs one of two actions depending on the given Boolean value.
		/// </summary>
		/// <param name="isTrue">The Boolean object made fluent using PipelineStyle.</param>
		/// <param name="onTrue">Action to invoke.</param>
		/// <param name="onElse">Else action to invoke.</param>
		/// <returns>The Boolean object passed in.</returns>
		public 
		static 
		bool
		IfDo(
			this bool							isTrue,
			Action								onTrue,
			Action								onElse)
		{
			if (isTrue)
				onTrue();
			else 
				onElse();

			return isTrue;
		}


		// -------------------------------------------------------------------------------------------------------------
		// -------------------------------------------------------------------------------------------------------------
		// To Section
		// -------------------------------------------------------------------------------------------------------------
		// -------------------------------------------------------------------------------------------------------------
		// 'To' takes an object, applies a function, and return another object.

		/// <summary>
		/// Applies a function to the given object.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="func">Function to invoke.</param>
		/// <returns>The result of the function called.</returns>
		public 
		static
		TOut
		To<TIn, TOut>(
			this TIn							obj,
			Func<TIn, TOut>						func)
		{
			return func(obj);
		}

		// -------------------------------------------------------------------------------------------------------------
		// To - If

		/// <summary>
		/// Applies a function to the given object.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="isTrue">Boolean value to determine if the action should be invoked.</param>
		/// <param name="onTrue">Function to invoke.</param>
		/// <returns>The result of the function if called, or the corresponding default value.</returns>
		public 
		static
		TOut
		ToIf<TIn, TOut>(
			this TIn							obj,
			bool								isTrue,
			Func<TIn, TOut>						onTrue)
		{
			return	isTrue
						?	onTrue(obj)
						:	default(TOut);
		}

		/// <summary>
		/// Applies a function to the given object.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="isTrue">Predicate to which the object is passed to determine if the function should be invoked.</param>
		/// <param name="onTrue">Function to invoke.</param>
		/// <returns>The result of the function if called, or the corresponding default value.</returns>
		public 
		static
		TOut
		ToIf<TIn, TOut>(
			this TIn							obj,
			Func<TIn, bool>						isTrue,
			Func<TIn, TOut>						onTrue)
		{
			return	isTrue(obj)
						?	onTrue(obj)
						:	default(TOut);
		}

		/// <summary>
		/// Applied one of two functions to the given object, depending on the given predicate.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="isTrue">Predicate to which the object is passed to determine if the function should be invoked.</param>
		/// <param name="onTrue">Function to invoke if the predicate return true.</param>
		/// <param name="onFalse">Function to invoke if the predicate returns false.</param>
		/// <returns>The result of the function called.</returns>
		public 
		static
		TOut
		ToIf<TIn, TOut>(
			this TIn							obj,
			Func<TIn, bool>						isTrue,
			Func<TIn, TOut>						onTrue,
			Func<TIn, TOut>						onFalse)
		{
			return	isTrue(obj)
						?	onTrue(obj)
						:	onFalse(obj);
		}

		/// <summary>
		/// Applied one of two functions to the given object, depending on the given Boolean value.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="isTrue">Boolean value to determine which function should be invoked.</param>
		/// <param name="onTrue">Function to invoke if true.</param>
		/// <param name="onFalse">Function to invoke if false.</param>
		/// <returns>The result of whichever function was called.</returns>
		public 
		static
		TOut
		ToIf<TIn, TOut>(
			this TIn							obj,
			bool								isTrue,
			Func<TIn, TOut>						onTrue,
			Func<TIn, TOut>						onFalse)
		{
			return	isTrue
						?	onTrue(obj)
						:	onFalse(obj);
		}

		/// <summary>
		/// Applies a function to the given object.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="isTrue">Predicate to which the object is passed to determine if the function should be invoked.</param>
		/// <param name="onTrue">Function to invoke.</param>
		/// <param name="falseValue">Value to return if the predicate returns false.</param>
		/// <returns>The result of the function called or the falseValue.</returns>
		public 
		static
		TOut
		ToIf<TIn, TOut>(
			this TIn							obj,
			Func<TIn, bool>						isTrue,
			Func<TIn, TOut>						onTrue,
			TOut								falseValue)
		{
			return	isTrue(obj)
						?	onTrue(obj)
						:	falseValue;
		}


		/// <summary>
		/// Applies a function to the given object.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="isTrue">Boolean value to determine if function should be invoked.</param>
		/// <param name="onTrue">Function to invoke.</param>
		/// <param name="falseValue">Value to return if the predicate returns false.</param>
		/// <returns>The result of the function called or the falseValue.</returns>
		public 
		static
		TOut
		ToIf<TIn, TOut>(
			this TIn							obj,
			bool								isTrue,
			Func<TIn, TOut>						onTrue,
			TOut								falseValue)
		{
			return	isTrue
						?	onTrue(obj)
						:	falseValue;
		}

		/// <summary>
		/// Applied one of two functions to the given object, depending on the given predicate.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="isTrue">Boolean value to determine which function should be invoked.</param>
		/// <param name="onTrue">Function to invoke if the Boolean value is true.</param>
		/// <param name="onFalse">Function to invoke if Boolean value is false.</param>
		/// <returns>The result of the function called.</returns>
		public 
		static
		TOut
		ToIf<TIn, TOut>(
			this TIn							obj,
			bool								isTrue,
			Func<TIn, TOut>						onTrue,
			Func<TOut>							onFalse)
		{
			return	isTrue
						?	onTrue(obj)
						:	onFalse();
		}

		// -------------------------------------------------------------------------------------------------------------
		// To - Null 

		/// <summary>
		/// Applies a function to the given object, if the object is not null.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNotNull">Function to invoke.</param>
		/// <returns>The result of the function if called, or the corresponding default value.</returns>
		public 
		static 
		TOut
		ToNotNull<TIn, TOut>(
			this TIn							obj,
			Func<TIn, TOut>						onNotNull)
		where 
			TIn								:	class 
		{
			return	obj != null
						?	onNotNull(obj)
						:	default(TOut);
		}

		/// <summary>
		/// Applies one of two function to the given object depending on whether the object is null or not.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNotNull">Function to invoke if object in not null.</param>
		/// <param name="onElse">Function to invoke if object is null.</param>
		/// <returns>The result of the function called.</returns>
		public 
		static 
		TOut
		ToNotNull<TIn, TOut>(
			this TIn							obj,
			Func<TIn, TOut>						onNotNull,
			Func<TOut>							onElse)
		where 
			TIn								:	class 
		{
			return	obj != null
						?	onNotNull(obj)
						:	onElse();
		}

		/// <summary>
		/// Applies a function to the given object, if the object is not null.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNotNull">Function to invoke.</param>
		/// <param name="defaultValue">Value to return if object is null.</param>
		/// <returns>The result of the function if called, or the defaultValue.</returns>
		public 
		static 
		TOut
		ToNotNull<TIn, TOut>(
			this TIn							obj,
			Func<TIn, TOut>						onNotNull,
			TOut								defaultValue)
		where 
			TIn								:	class 
		{
			return	obj != null
						?	onNotNull(obj)
						:	defaultValue;
		}

		/// <summary>
		/// Calls a function if the object is null.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNull">Function to invoke.</param>
		/// <returns>The result of the function if called or the object.</returns>
		public 
		static 
		T
		ToIsNull<T>(
			this T								obj,
			Func<T>								onNull)
		where 
			T								:	class 
		{
			return	obj ?? onNull();
		}

		// -------------------------------------------------------------------------------------------------------------
		// To - Default

		/// <summary>
		/// Applies a function to the given object, if the object is not the default.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNotDefault">Function to invoke.</param>
		/// <returns>The result of the function if called, or the corresponding default value.</returns>
		public 
		static 
		TOut
		ToNotDefault<TIn, TOut>(
			this TIn							obj,
			Func<TIn, TOut>						onNotDefault)
		{
			return	!obj.IsDefault()
						?	onNotDefault(obj)
						:	default(TOut);
		}

		/// <summary>
		/// Applies one of two functions to the given object depending on whether the object is the default value or not.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onNotDefault">Function to invoke.</param>
		/// <param name="onElse">Function to invoke if object is the default value.</param>
		/// <returns>The result of the function called.</returns>
		public 
		static 
		TOut
		ToNotDefault<TIn, TOut>(
			this TIn							obj,
			Func<TIn, TOut>						onNotDefault,
			Func<TOut>							onElse)
		{
			return	!obj.IsDefault()
						?	onNotDefault(obj)
						:	onElse();
		}


		/// <summary>
		/// Calls a function if the object is the default value.
		/// </summary>
		/// <param name="obj">The object made fluent using PipelineStyle.</param>
		/// <param name="onDefault">Function to invoke.</param>
		/// <returns>The result of the function if called or the object.</returns>
		public 
		static 
		T
		ToIsDefault<T>(
			this T								obj,
			Func<T>								onDefault)
		{
			return	obj.IsDefault()
						?	onDefault()
						:	obj;
		}

		// -------------------------------------------------------------------------------------------------------------
		// If - To

		/// <summary>
		/// Calls a function if the given Boolean value is true.
		/// </summary>
		/// <param name="isTrue">The Boolean object made fluent using PipelineStyle.</param>
		/// <param name="onTrue">Function to invoke.</param>
		/// <returns>The result of the function if called, or the corresponding default value.</returns>
		public 
		static 
		T
		IfTo<T>(
			this bool							isTrue,
			Func<T>								onTrue)
		{
			return	isTrue
						?	onTrue()
						:	default(T);
		}

		/// <summary>
		/// Calls one of two functions depending on the given Boolean value.
		/// </summary>
		/// <param name="isTrue">The Boolean object made fluent using PipelineStyle.</param>
		/// <param name="onTrue">Function to invoke if Boolean value is true.</param>
		/// <param name="onElse">Function to invoke if Boolean value is false.</param>
		/// <returns>The result of the function called.</returns>
		public 
		static 
		T
		IfTo<T>(
			this bool							isTrue,
			Func<T>								onTrue,
			Func<T>								onElse)
		{
			return	isTrue
						?	onTrue()
						:	onElse();
		}

		/// <summary>
		/// Calls a function if the given Boolean value is true.
		/// </summary>
		/// <param name="isTrue">The Boolean object made fluent using PipelineStyle.</param>
		/// <param name="onTrue">Function to invoke if Boolean value is true.</param>
		/// <param name="elseValue">Value returned if Boolean value is false.</param>
		/// <returns>The result of the function if called or the elseValue.</returns>
		public 
		static 
		T
		IfTo<T>(
			this bool							isTrue,
			Func<T>								onTrue,
			T									elseValue)
		{
			return	isTrue
						?	onTrue()
						:	elseValue;
		}

		// -------------------------------------------------------------------------------------------------------------
		// ToUsing

		/// <summary>
		/// Calls the function, passing it the object to be used in the using statement. 
		/// Automatically calls the dispose method on exist.
		/// </summary>
		/// <param name="obj">The object to be used in the 'using statement'</param>
		/// <param name="work">The body of the using statement, passed in as a function.</param>
		/// <returns>Result of the function called.</returns>
		public 
		static 
		TOut
		ToUsing<TIn, TOut>(
			this TIn							obj,
			Func<TIn, TOut>						work)
		where 
			TIn								:	IDisposable
		{
			using(obj)
				return work(obj);
		}
	}

	// -----------------------------------------------------------------------------------------------------------------
	// -----------------------------------------------------------------------------------------------------------------

	public 
	class DummyAttribute					:	Attribute
	{
	}

	// -----------------------------------------------------------------------------------------------------------------
	// -----------------------------------------------------------------------------------------------------------------
}
