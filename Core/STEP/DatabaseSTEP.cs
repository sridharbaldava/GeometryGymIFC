// MIT License
// Copyright (c) 2016 Geometry Gym Pty Ltd

// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using GeometryGym.STEP;
using System.Collections;


namespace GeometryGym.STEP
{
	public partial class DatabaseSTEP<T> : IEnumerable<T> where T : STEPEntity//, new()
	{
		internal string mFileName = "";
		public string FileName
		{
			get { return mFileName; }
			set { mFileName = value; if(!string.IsNullOrEmpty(value)) FolderPath = Path.GetDirectoryName(value); }
		}

		public DatabaseSTEP() { mNextBlank = 1; }

		private SortedDictionary<int, T> mObjects = new SortedDictionary<int, T>();
		public int LastKey() { return mObjects.Count == 0 ? 0 : mObjects.Last().Key; }

		public int NextObjectRecord { set { mNextBlank = value; } }
		public virtual T this[int index]
		{
			get
			{
				T result = null;
				mObjects.TryGetValue(index, out result);
				return result;
			}
			set
			{
				if (value == null)
				{
					if (mObjects.ContainsKey(index))
						mObjects.Remove(index);
					if (index < mNextBlank && index > 0)
						mNextBlank = index;
					return;
				}
				
				mObjects[index] = value;
				if (index == mNextBlank)
					mNextBlank = mNextBlank + 1;
				value.mIndex = index;
			}
		}
		internal void appendObject(T o) { this[NextBlank()] = o; }
		private int mNextBlank
		{
			get;
			set;
		} 
		internal int NextBlank()
		{
			while (mObjects.ContainsKey(mNextBlank))
				mNextBlank++;
			return mNextBlank;
		}
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return mObjects.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return mObjects.Values.GetEnumerator();
		}
		private SortedSet<string> mParsingErrors = new SortedSet<string>();
		private SortedSet<string> mParsingWarnings = new SortedSet<string>();
		internal void logParseError(string str) { string error = "XX Error " + str;  if (!mParsingErrors.Contains(error)) mParsingErrors.Add(error); }
		internal void logParseWarning(string str) { string warning = "!! Warning " + str; if (!mParsingWarnings.Contains(warning)) mParsingWarnings.Add(warning); }

		public string FolderPath { get; set; } 
		internal string PreviousApplication { get; set; }
	}
}

 