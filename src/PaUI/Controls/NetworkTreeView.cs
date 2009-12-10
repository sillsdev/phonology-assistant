using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SIL.Localize.LocalizationUtils;

namespace SIL.Pa.UI.Controls
{
	#region NetworkTreeView class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NetworkTreeView : TreeView
	{
		internal static string s_msWindowsNetworkName = "Microsoft Windows Network";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			ImageList = new ImageList();
			ImageList.ColorDepth = ColorDepth.Depth32Bit;
			ImageList.Images.Add(Properties.Resources.kimidEntireNetwork);
			ImageList.Images.Add(Properties.Resources.kimidMicrosoftWindowsNetwork);
			ImageList.Images.Add(Properties.Resources.kimidMyNetworkPlaces);
			ImageList.Images.Add(Properties.Resources.kimidNetworkGroup);
			ImageList.Images.Add(Properties.Resources.kimidNetworkServer);

			AddTopNodes();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Load(string msWindowsNetworkName)
		{
			if (!string.IsNullOrEmpty(msWindowsNetworkName))
				s_msWindowsNetworkName = msWindowsNetworkName;

			Load();
		}
		
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AddTopNodes()
		{
			Nodes.Clear();

			// Add the current machine to the top of the list.
			NetworkTreeNode node = new NetworkTreeNode();
			node.ImageIndex = node.SelectedImageIndex = 4;
			node.Text = NetworkTreeNode.GetDisplayName(Environment.MachineName);
			node.MachineName = Environment.MachineName;
			node.NodeType = NetworkTreeNode.NetResTreeNodeType.Machine;
			Nodes.Add(node);

			// Add a node that is the parent of the computer's in the user's domain
			// as well as the "Microsoft Terminal Services" node, and the "Web
			// Client Network".
			node = new NetworkTreeNode();
			node.ImageIndex = node.SelectedImageIndex = 2;
			node.Text = Properties.Resources.kstidMyNetworkPlaces;
			node.NodeType = NetworkTreeNode.NetResTreeNodeType.PlacesInArea;
			Nodes.Add(node);

			node.Nodes.Add(NetworkTreeNode.NewDummyNode);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
		{
			NetworkTreeNode node = e.Node as NetworkTreeNode;
			if (node != null && !node.Populated)
				node.PopulateChildren();

			base.OnBeforeExpand(e);
		}
	}

	#endregion

	#region NETRESOURCE class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[StructLayout(LayoutKind.Sequential)]
	internal class NETRESOURCE
	{
		public NetworkTreeNode.ResourceScope dwScope = 0;
		public NetworkTreeNode.ResourceType dwType = 0;
		public NetworkTreeNode.ResourceDisplayType dwDisplayType = 0;
		public NetworkTreeNode.ResourceUsage dwUsage = 0;
		public string lpLocalName;
		public string lpRemoteName;
		public string lpComment;
		public string lpProvider;
	}

	#endregion

	#region NetworkTreeNode class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NetworkTreeNode : TreeNode
	{
		#region Enumerations
		internal enum NetResTreeNodeType
		{
			Dummy,
			Group,
			Machine,
			PlacesInArea
		}

		public enum ResourceScope
		{
			RESOURCE_CONNECTED = 1,
			RESOURCE_GLOBALNET,
			RESOURCE_REMEMBERED,
			RESOURCE_RECENT,
			RESOURCE_CONTEXT
		};

		public enum ResourceType
		{
			RESOURCETYPE_ANY,
			RESOURCETYPE_DISK,
			RESOURCETYPE_PRINT,
			RESOURCETYPE_RESERVED
		};

		public enum ResourceUsage
		{
			RESOURCEUSAGE_CONNECTABLE = 0x00000001,
			RESOURCEUSAGE_CONTAINER = 0x00000002,
			RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
			RESOURCEUSAGE_SIBLING = 0x00000008,
			RESOURCEUSAGE_ATTACHED = 0x00000010,
			RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
		};

		public enum ResourceDisplayType
		{
			RESOURCEDISPLAYTYPE_GENERIC,
			RESOURCEDISPLAYTYPE_DOMAIN,
			RESOURCEDISPLAYTYPE_SERVER,
			RESOURCEDISPLAYTYPE_SHARE,
			RESOURCEDISPLAYTYPE_FILE,
			RESOURCEDISPLAYTYPE_GROUP,
			RESOURCEDISPLAYTYPE_NETWORK,
			RESOURCEDISPLAYTYPE_ROOT,
			RESOURCEDISPLAYTYPE_SHAREADMIN,
			RESOURCEDISPLAYTYPE_DIRECTORY,
			RESOURCEDISPLAYTYPE_TREE,
			RESOURCEDISPLAYTYPE_NDSCONTAINER
		};

		internal enum ErrorCodes
		{
			NO_ERROR = 0,
			ERROR_NO_MORE_ITEMS = 259
		};

		#endregion

		#region Imported functions
		[DllImport("Mpr.dll", EntryPoint = "WNetOpenEnumA", CallingConvention = CallingConvention.Winapi)]
		private static extern ErrorCodes WNetOpenEnum(ResourceScope dwScope, ResourceType dwType,
			ResourceUsage dwUsage, NETRESOURCE p, out IntPtr lphEnum);

		[DllImport("Mpr.dll", EntryPoint = "WNetCloseEnum", CallingConvention = CallingConvention.Winapi)]
		private static extern ErrorCodes WNetCloseEnum(IntPtr hEnum);

		[DllImport("Mpr.dll", EntryPoint = "WNetEnumResourceA", CallingConvention = CallingConvention.Winapi)]
		private static extern ErrorCodes WNetEnumResource(IntPtr hEnum, ref uint lpcCount,
			IntPtr buffer, ref uint lpBufferSize);

		#endregion

		internal bool Populated;
		internal NETRESOURCE NetResource;
		internal NetResTreeNodeType NodeType;
		public string MachineName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal void PopulateChildren()
		{
			if (Populated || NodeType == NetResTreeNodeType.Machine ||
				NodeType == NetResTreeNodeType.Dummy)
			{
				return;
			}

			TreeView.FindForm().Cursor = Cursors.WaitCursor;
			Nodes.Clear();
			NetworkTreeNode childNode;
			NETRESOURCE netRes = NetResource;

			// When the node being expanded is places near the user's computer, (which
			// is sort of like My Network Places) then add a node below it for the
			// entire network, then find the network resource for the same domain in
			// which the user's machine is a member.
			if (NodeType == NetResTreeNodeType.PlacesInArea)
			{
				childNode = new NetworkTreeNode();
				childNode.NodeType = NetResTreeNodeType.Group;
				childNode.ImageIndex = childNode.SelectedImageIndex = 0;
				childNode.Nodes.Add(NewDummyNode);
				childNode.Text = LocalizationManager.LocalizeString(
					"EntireNetworkNode", "Entire network", "Text in the network tree used " +
					"when trying to locate a FieldWorks project older than version 7.0.",
					"Dialog Boxes.FieldWorks Project", LocalizationCategory.Other,
					LocalizationPriority.Medium);

				Nodes.Add(childNode);

				netRes = FindSameDomainResource(null);
			}
			
			// Get all the network resources that are subordinate to the current node's.
			List<NETRESOURCE> netResources = GetChildResources(netRes);

			if (netResources != null && netResources.Count > 0)
			{
				foreach (NETRESOURCE nr in netResources)
				{
					if (string.IsNullOrEmpty(nr.lpRemoteName))
						continue;

					childNode = new NetworkTreeNode();
					childNode.NetResource = nr;
					childNode.Text = GetDisplayName(nr);

					if (nr.dwDisplayType == ResourceDisplayType.RESOURCEDISPLAYTYPE_SERVER)
					{
						childNode.ImageIndex = childNode.SelectedImageIndex = 4;
						childNode.MachineName = childNode.Text;
						childNode.NodeType = NetResTreeNodeType.Machine;
					}
					else
					{
						childNode.ImageIndex = childNode.SelectedImageIndex = 
							(nr.dwDisplayType == ResourceDisplayType.RESOURCEDISPLAYTYPE_NETWORK ?
							1 : 3);

						childNode.NodeType = NetResTreeNodeType.Group;
						childNode.Nodes.Add(NewDummyNode);
					}

					Nodes.Add(childNode);
				}
			}

			Populated = true;
			NetResource = null;
			TreeView.FindForm().Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Take off the wack, wack and make only the first letter uppercase.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static string GetDisplayName(NETRESOURCE netRes)
		{
			return GetDisplayName(netRes.lpRemoteName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Take off the wack, wack and make only the first letter uppercase.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static string GetDisplayName(string name)
		{
			name = name.TrimStart("\\".ToCharArray());
			return name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Dummy nodes are nodes added to a superior node that is empty (i.e. not populated)
		/// so the supperior node will be displayed in the tree with a + next to it. When
		/// the superior node is expanded, the dummy node is removed and replaced with all
		/// the "real" subordinate nodes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static NetworkTreeNode NewDummyNode
		{
			get
			{
				NetworkTreeNode node = new NetworkTreeNode();
				node.NodeType = NetResTreeNodeType.Dummy;
				return node;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static List<NETRESOURCE> GetChildResources(NETRESOURCE parentNetRes)
		{
			if (parentNetRes == null)
				parentNetRes = new NETRESOURCE();

			uint bufferSize = 16384;
			IntPtr buffer = Marshal.AllocHGlobal((int)bufferSize);
			IntPtr handle;
			uint cEntries = 1;
			List<NETRESOURCE> netResources = new List<NETRESOURCE>();

			ErrorCodes result = WNetOpenEnum(ResourceScope.RESOURCE_GLOBALNET,
				ResourceType.RESOURCETYPE_ANY, ResourceUsage.RESOURCEUSAGE_ALL,
				parentNetRes, out handle);

			if (result == ErrorCodes.NO_ERROR)
			{
				do
				{
					result = WNetEnumResource(handle, ref cEntries, buffer, ref	bufferSize);

					if (result == ErrorCodes.NO_ERROR)
					{
						NETRESOURCE netRes = new NETRESOURCE();
						Marshal.PtrToStructure(buffer, netRes);

						if (netRes.lpRemoteName == NetworkTreeView.s_msWindowsNetworkName &&
							netRes.dwDisplayType == ResourceDisplayType.RESOURCEDISPLAYTYPE_NETWORK)
						{
							netResources.AddRange(GetChildResources(netRes));
						}
						else
						{
							netResources.Add(netRes);
						}
					}
					else if (result != ErrorCodes.ERROR_NO_MORE_ITEMS)
						break;

				} while (result != ErrorCodes.ERROR_NO_MORE_ITEMS);

				WNetCloseEnum(handle);
			}

			Marshal.FreeHGlobal(buffer);
			return netResources;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Finds the network resource of the domain in which the current machine is a member.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static NETRESOURCE FindSameDomainResource(NETRESOURCE parentNetRes)
		{
			ResourceScope scope = ResourceScope.RESOURCE_GLOBALNET;

			if (parentNetRes == null)
				parentNetRes = new NETRESOURCE();

			uint bufferSize = 16384;
			IntPtr buffer = Marshal.AllocHGlobal((int)bufferSize);
			IntPtr handle;
			uint cEntries = 1;
			NETRESOURCE retRes = null;

			ErrorCodes result = WNetOpenEnum(scope, ResourceType.RESOURCETYPE_ANY,
				ResourceUsage.RESOURCEUSAGE_ALL, parentNetRes, out handle);

			if (result == ErrorCodes.NO_ERROR)
			{
				do
				{
					result = WNetEnumResource(handle, ref cEntries, buffer, ref	bufferSize);

					if (result == ErrorCodes.NO_ERROR)
					{
						NETRESOURCE netRes = new NETRESOURCE();
						Marshal.PtrToStructure(buffer, netRes);

						if (netRes.dwDisplayType != ResourceDisplayType.RESOURCEDISPLAYTYPE_SERVER &&
							netRes.dwDisplayType != ResourceDisplayType.RESOURCEDISPLAYTYPE_GROUP &&
							netRes.dwDisplayType != ResourceDisplayType.RESOURCEDISPLAYTYPE_DOMAIN)
						{
							retRes = FindSameDomainResource(netRes);
						}
						else if (netRes.lpRemoteName == Environment.UserDomainName &&
							(netRes.dwDisplayType == ResourceDisplayType.RESOURCEDISPLAYTYPE_GROUP ||
							netRes.dwDisplayType == ResourceDisplayType.RESOURCEDISPLAYTYPE_DOMAIN))
						{
							retRes = netRes;
						}
					}
					else if (result != ErrorCodes.ERROR_NO_MORE_ITEMS)
						break;

				} while (result != ErrorCodes.ERROR_NO_MORE_ITEMS && retRes == null);

				WNetCloseEnum(handle);
			}

			Marshal.FreeHGlobal(buffer);
			return retRes;
		}
	}

	#endregion
}
