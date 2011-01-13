// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright from='2009' to='2009' company='SIL International'>
//		Copyright (c) 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: MsgDispatcher.cs
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SilTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class MessageDispatcher
	{
		//internal struct QueuedMsg
		//{
		//    internal string Msg;
		//    internal object Sender;
		//    internal object Args;
		//}

		//private string m_msgCurrentlyBeingDispateched = null;
		private readonly HashSet<object> m_dispatchReceivers = new HashSet<object>();
		//private List<QueuedMsg> m_msgQueue = new List<QueuedMsg>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether dispatching should be suspended.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool SuspendDispatching { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether the specified receiver has been added to the dispatcher.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool HasReceiver(object receiver)
		{
			return m_dispatchReceivers.Contains(receiver);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified receiver to the list of receivers to which messages
		///  are dispatched.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddReceiver(object receiver)
		{
			if (receiver != null && !m_dispatchReceivers.Contains(receiver))
			{
				m_dispatchReceivers.Add(receiver);
				if (receiver is Control)
					((Control)receiver).Disposed += HandleReceiverDisposed;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the dispatch recipient.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RemoveReceiver(object receiver)
		{
			if (m_dispatchReceivers.Contains(receiver))
			{
				m_dispatchReceivers.Remove(receiver);
				if (receiver is Control)
					((Control)receiver).Disposed -= HandleReceiverDisposed;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Removes the disposing recipient.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleReceiverDisposed(object sender, EventArgs e)
		{
			RemoveReceiver(sender);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sends a message. This overload will skip dispatching the message to the sender
		/// if the sender is also in the list of receivers.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SendMessage(string msg, object sender, object args)
		{
			SendMessage(msg, sender, args, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sends a message.
		/// </summary>
		/// <param name="msg">The Message without the "On" prefix.</param>
		/// <param name="sender">The calling object instance.</param>
		/// <param name="args">The message arguments.</param>
		/// <param name="skipSender">if set to <c>true</c> messages will not be sent to
		/// the sender if the sender is also in the receiver list.</param>
		/// ------------------------------------------------------------------------------------
		public void SendMessage(string msg, object sender, object args, bool skipSender)
		{
			if (SuspendDispatching)
				return;

			foreach (var receiver in m_dispatchReceivers)
			{
				if (skipSender && receiver == sender)
					continue;

				if (DispatchMessage(receiver, msg, sender, args))
					break;
			}
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Sends the message.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public void SendMessage(string msg, object sender, object args)
		//{
		//    SendMessage(msg, sender, args, true);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Sends the message.
		///// </summary>
		///// <param name="msg">The Message without the "On" prefix.</param>
		///// <param name="sender">The calling object instance.</param>
		///// <param name="args">The message arguments.</param>
		///// <param name="ignoreDupMsgs">if set to <c>true</c> all messages of the same name
		///// will be ignored while the current message is being dispatched.</param>
		///// ------------------------------------------------------------------------------------
		//public void SendMessage(string msg, object sender, object args, bool ignoreDupMsgs)
		//{
		//    if (SuspendDispatching || (ignoreDupMsgs && msg == m_msgCurrentlyBeingDispateched))
		//        return;

		//    m_msgQueue.Add(new QueuedMsg {Msg = msg, Sender = sender, Args = args});
		//    //if (m_msgQueue.Count > 1)
		//    //    return;

		//    while (m_msgQueue.Count > 0)
		//    {
		//        QueuedMsg qm = m_msgQueue[0];
		//        m_msgCurrentlyBeingDispateched = qm.Msg;

		//        foreach (var receiver in m_dispatchReceivers)
		//        {
		//            if (DispatchMessage(receiver, qm.Msg, qm.Sender, qm.Args))
		//                break;
		//        }

		//        m_msgCurrentlyBeingDispateched = null;
		//        m_msgQueue.RemoveAt(0);
		//    }
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Dispatches the message.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool DispatchMessage(object receiver, string msg, object sender, object args)
		{
			if (msg == null)
				throw new ArgumentNullException("msg");

			if (sender == null)
				throw new ArgumentNullException("sender");

			if (msg.StartsWith("On"))
				throw new ArithmeticException("Messages should not begin with 'On'");

			msg = "On" + msg;
			if (!DoesReceiverHaveMatchingHandler(receiver, msg))
				return false;

			return ReflectionHelper.GetBoolResult(receiver, msg, new[] { sender, args });
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines if the specified receiver contains a method with the proper signature.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static bool DoesReceiverHaveMatchingHandler(object receiver, string msg)
		{
			const BindingFlags flags = BindingFlags.Instance |
				BindingFlags.NonPublic | BindingFlags.Public;

			MethodInfo mi = receiver.GetType().GetMethod(msg, flags);

			// Make sure method on the receiver exists and check for proper return type.
			if (mi == null || mi.ReturnType != typeof(bool))
				return false;

			// Check for proper method signiture;
			ParameterInfo[] parms = mi.GetParameters();
			if (parms.Length != 2 &&
				parms[0].ParameterType != typeof(object) &&
				parms[1].ParameterType != typeof(object))
			{
				return false;
			}

			return true;
		}
	}
}
