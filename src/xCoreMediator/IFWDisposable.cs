// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2007, SIL International. All Rights Reserved.
// <copyright from='2007' to='2007' company='SIL International'>
//		Copyright (c) 2007, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: IFWDisposable.cs
// Responsibility: TE Team
// 
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace SIL.FieldWorks.Common.Utils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Simple interface to extend the IDisposable interface by adding the IsDisposed
	/// and CheckDisposed properties.  These methods exist so that we can make sure
	/// that objects are being used only while they are valid, not disposed.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IFWDisposable : IDisposable
	{
		/// <summary>
		/// Add the public property for knowing if the object has been disposed of yet
		/// </summary>
		bool IsDisposed { get;}

		/// <summary>
		/// This method throws an ObjectDisposedException if IsDisposed returns 
		/// true.  This is the case where a method or property in an object is being 
		/// used but the object itself is no longer valid.  
		/// 
		/// This method should be added to all public properties and methods of this
		/// object and all other objects derived from it (extensive).
		/// </summary>
		void CheckDisposed();
		// Sample implementation:
		// {
		//    if (IsDisposed)
		//        throw new ObjectDisposedException("ObjectName", 
		//            "This object is being used after it has been disposed: this is an Error.");
		// }
	}

}
