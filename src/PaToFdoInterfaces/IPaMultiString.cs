namespace SIL.PaToFdoInterfaces
{
	#region IPaMultiString interface
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Interface for returning information for a FW multi. string.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public interface IPaMultiString
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the string for the writing system having the specified Id.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		string GetString(string wsId);
	}

	#endregion
}
