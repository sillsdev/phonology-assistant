using System.Collections.Generic;

namespace SIL.Pa
{
	public class WordCache : List<WordCacheEntry>
	{
		//private string m_wordToFind = null;
		//private int m_idToFind = -1;

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Returs the first word record in the cache whose phonetic word matches the one
		///// specified.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public WordCacheEntry this[string eticWord]
		//{
		//    get
		//    {
		//        m_idToFind = -1;
		//        m_wordToFind = eticWord;
		//        return Find(WordFinder);
		//    }
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// The predicate delegate used when using the this[string] and this[int] accessor.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//private bool WordFinder(WordCacheEntry rec)
		//{
		//    return (m_idToFind < 0 ?
		//        (rec.Phonetic == m_wordToFind) : (rec.Id == m_idToFind));
		//}
	}
}
