using System.Windows.Forms;
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
			m_dispatcher.AddReceiver(m_receiver);
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that receiver classes are added and removed properly.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void ReceiverAddedAndRemoved()
		{
			m_dispatcher.AddReceiver(this);
			Assert.IsTrue(m_dispatcher.HasReceiver(this));
			m_dispatcher.RemoveReceiver(this);
			Assert.IsFalse(m_dispatcher.HasReceiver(this));
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that receiver classes are removed from the dispatcher when they are disposed.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void ReceiverRemovedWhenDisposed()
		{
			m_dispatcher.AddReceiver(m_receiver);
			Assert.IsTrue(m_dispatcher.HasReceiver(m_receiver));
			m_receiver.Dispose();
			Assert.IsFalse(m_dispatcher.HasReceiver(m_receiver));
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that receivers of public, private, protected and internal are called.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void AllReceiversCalled()
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
		public void ReceiverArgsReceived()
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
		public void DispatchingInterupted()
		{
			var firstReceiver = new ReceiverReturningTrue();
			m_dispatcher.RemoveReceiver(m_receiver);
			m_dispatcher.AddReceiver(firstReceiver);
			m_dispatcher.AddReceiver(m_receiver);

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

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that when dispatching messages, the sender of the message is not included
		/// in the message dispatching.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void SkipSender()
		{
			var receiver2 = new ReceiverReturningTrue();
			m_dispatcher.AddReceiver(receiver2);

			Assert.IsNull(receiver2.Args);
			Assert.IsNull(receiver2.Sender);
			Assert.IsNull(m_receiver.Args);
			Assert.IsNull(m_receiver.Sender);

			// Call without the skip flag, since the default behavior is to skip.
			m_dispatcher.SendMessage("ProtectedMsgHandled", m_receiver, "Eeee!");

			Assert.IsNull(m_receiver.Args);
			Assert.IsNull(m_receiver.Sender);
			Assert.AreEqual("Eeee!", receiver2.Args);
			Assert.AreEqual(m_receiver, receiver2.Sender);

			receiver2.Args = null;
			receiver2.Sender = null;

			// Call with the skip flag.
			m_dispatcher.SendMessage("ProtectedMsgHandled", m_receiver, "Eeee!", true);

			Assert.IsNull(m_receiver.Args);
			Assert.IsNull(m_receiver.Sender);
			Assert.AreEqual("Eeee!", receiver2.Args);
			Assert.AreEqual(m_receiver, receiver2.Sender);
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that when dispatching messages, the sender of the message is included
		/// in the message dispatching.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void IncludeSender()
		{
			var receiver2 = new ReceiverReturningTrue();
			m_dispatcher.AddReceiver(receiver2);

			Assert.IsNull(receiver2.Args);
			Assert.IsNull(receiver2.Sender);
			Assert.IsNull(m_receiver.Args);
			Assert.IsNull(m_receiver.Sender);

			// Call without the skip flag, expecting that default behavior is to skip.
			m_dispatcher.SendMessage("ProtectedMsgHandled", m_receiver, "Eeee!");

			Assert.IsNull(m_receiver.Args);
			Assert.IsNull(m_receiver.Sender);
			Assert.AreEqual("Eeee!", receiver2.Args);
			Assert.AreEqual(m_receiver, receiver2.Sender);

			receiver2.Args = null;
			receiver2.Sender = null;

			// Now call with the skip flag set to false.
			m_dispatcher.SendMessage("ProtectedMsgHandled", m_receiver, "Eeee!", false);

			Assert.AreEqual("Eeee!", m_receiver.Args);
			Assert.AreEqual(m_receiver, m_receiver.Sender);
			Assert.AreEqual("Eeee!", receiver2.Args);
			Assert.AreEqual(m_receiver, receiver2.Sender);
		}

		///--------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that suspending message dispatching actually works.
		/// </summary>
		///--------------------------------------------------------------------------------------
		[Test]
		public void DispatchingSuspended()
		{
			Assert.IsNull(m_receiver.Args);
			Assert.IsNull(m_receiver.Sender);

			m_dispatcher.SendMessage("InternalMsgHandled", this, null);
			Assert.AreEqual(this, m_receiver.Sender);
			m_receiver.Sender = null;

			m_dispatcher.SuspendDispatching = true;
			m_dispatcher.SendMessage("InternalMsgHandled", this, null);
			Assert.IsNull(m_receiver.Sender);
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
	/// Inherit from Control so the class supports the Dispose event.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	internal class ReceiverBase : Control
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
