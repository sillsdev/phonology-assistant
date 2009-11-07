using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SilUtils
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class MessageDispatcherTests
	{
		private MessageDispatcher m_dispatcher;
		private ReceiverBase m_receiver;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the setup.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_dispatcher = new MessageDispatcher();
			m_receiver = new ReceiverBase();
			m_dispatcher.AddDispatchReceiver(m_receiver);
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that receiver classes are added and removed properly.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void TestReceiverAddedAndRemoved()
		{
			m_dispatcher.AddDispatchReceiver(this);
			
			var receiverList =
				ReflectionHelper.GetField(m_dispatcher, "m_dispatchReceivers") as HashSet<object>;
			
			Assert.IsTrue(receiverList.Contains(this));
			m_dispatcher.RemoveDispatchReceiver(this);
			Assert.IsFalse(receiverList.Contains(this));
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that receivers of public, private, protected and internal are called.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void TestAllReceiversCalled()
		{
			var msgs = new[] { "PublicMsgHandled", "PrivateMsgHandled",
				"ProtectedMsgHandled", "InternalMsgHandled" };
		
			foreach (string msg in msgs)
			{
				m_receiver.Args = null;
				m_receiver.Sender = null;

				Assert.IsNull(m_receiver.Args);
				Assert.IsNull(m_receiver.Sender);

				m_dispatcher.SendMessage(msg, this, msg);

				Assert.AreEqual(msg, m_receiver.Args);
				Assert.AreEqual(this, m_receiver.Sender);
			}
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that receivers receive collection arguments.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void TestReceiverArgs()
		{
			Assert.IsNull(m_receiver.Args);
			Assert.IsNull(m_receiver.Sender);

			var args = new[] {100, 200};
			m_dispatcher.SendMessage("ProtectedMsgHandled", this, args);

			Assert.AreEqual(this, m_receiver.Sender);
			Assert.IsTrue(m_receiver.Args is int[]);
			Assert.AreEqual(100, ((int[])m_receiver.Args)[0]);
			Assert.AreEqual(200, ((int[])m_receiver.Args)[1]);
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that message handlers returning true interupt the dispatch cycle.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void TestDispatchingInterupted()
		{
			var firstReceiver = new ReceiverReturningTrue();
			m_dispatcher.RemoveDispatchReceiver(m_receiver);
			m_dispatcher.AddDispatchReceiver(firstReceiver);
			m_dispatcher.AddDispatchReceiver(m_receiver);

			Assert.IsNull(firstReceiver.Args);
			Assert.IsNull(firstReceiver.Sender);

			Assert.IsNull(m_receiver.Args);
			Assert.IsNull(m_receiver.Sender);

			m_dispatcher.SendMessage("ProtectedMsgHandled", this, "Aaaaahhhh!");

			Assert.IsNull(m_receiver.Args);
			Assert.IsNull(m_receiver.Sender);
			Assert.AreEqual("Aaaaahhhh!", firstReceiver.Args);
			Assert.AreEqual(this, firstReceiver.Sender);
		}
	}

	#region ReceiverReturningTrue class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class ReceiverReturningTrue : ReceiverBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Provides a message handler that will return true, thus interupting the dispatch
		/// cycle.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool OnProtectedMsgHandled(object sender, object args)
		{
			base.OnProtectedMsgHandled(sender, args);
			return true;
		}
	}

	#endregion

	#region ReceiverBase class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class ReceiverBase
	{
		internal object Args {get; set; }
		internal object Sender {get; set; }
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool OnPrivateMsgHandled(object sender, object args)
		{
			Sender = sender;
			Args = args;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal virtual bool OnInternalMsgHandled(object sender, object args)
		{
			Sender = sender;
			Args = args;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected virtual bool OnProtectedMsgHandled(object sender, object args)
		{
			Sender = sender;
			Args = args;
			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual bool OnPublicMsgHandled(object sender, object args)
		{
			Sender = sender;
			Args = args;
			return false;
		}
	}
	
	#endregion
}
