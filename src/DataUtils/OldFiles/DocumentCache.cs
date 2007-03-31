using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SIL.SpeechTools.Database
{
	public class DocumentCache : Dictionary<int, Document>
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// 
		/// </summary>
		/// <param name="docId"></param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public new Document this[int docId]
		{
			set {base[docId] = value;}
			get
			{
				if (!ContainsKey(docId))
				{
					Document docInfo = Document.Load(docId);
					if (docInfo == null)
						return null;

					base[docId] = docInfo;
				}

				return base[docId];
			}
		}

	//public class DocumentCache : Dictionary<int, DocumentInfo>
	//{
	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// 
	//    /// </summary>
	//    /// <param name="docId"></param>
	//    /// <returns></returns>
	//    /// ------------------------------------------------------------------------------------
	//    public new DocumentInfo this[int docId]
	//    {
	//        get
	//        {
	//            if (!ContainsKey(docId))
	//            {
	//                DocumentInfo doc = DocumentInfo.Load(docId);
	//                if (doc == null)
	//                    return null;

	//                base[docId] = doc;
	//            }

	//            return base[docId];
	//        }
	//        set {base[docId] = value;}
	//    }

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Adds a mew document to the document cache with the specified data.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public void AddNewDocument(Dictionary<string, string> rawData)
		//{
		//    int tmpId = GetTmpDocumentId();
		//    this[tmpId] = DocumentInfo.Create(tmpId, rawData);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a temporary id to user for a new document that will be added to the cache
		///// but hasn't yet been added to the database.
		///// </summary>
		///// <returns></returns>
		///// ------------------------------------------------------------------------------------
		//public int GetTmpDocumentId()
		//{
		//    // Temporary ids start at -1 and go down from there. Find the last negative
		//    // number used and return a number one less than that.
		//    int tmpId = 0;

		//    // See if the suggested id already exists.
		//    foreach (KeyValuePair<int, DocumentInfo> doc in this)
		//        tmpId = Math.Min(tmpId, doc.Value.Id);

		//    return --tmpId;
		//}
	}
}
