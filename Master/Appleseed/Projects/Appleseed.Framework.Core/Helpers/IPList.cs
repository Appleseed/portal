// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPList.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Helper for dealing with IP numbers and ranges of IP numbers.
//   Copyright by Bo Norgaard, All rights reserved.
//   Modified for Appleseed by Jeremy Esland (jes1111) 26/04/2005
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System;
    using System.Collections;
    using System.Text;

    /// <summary>
    /// Helper for dealing with IP numbers and ranges of IP numbers.
    ///   Copyright by Bo Norgaard, All rights reserved.
    ///   Modified for Appleseed by Jeremy Esland (jes1111) 26/04/2005
    /// </summary>
    public class IPList
    {
        #region Constants and Fields

        /// <summary>
        /// The ip range list.
        /// </summary>
        private readonly ArrayList ipRangeList = new ArrayList();

        /// <summary>
        /// The mask list.
        /// </summary>
        private readonly SortedList maskList = new SortedList();

        /// <summary>
        /// The used list.
        /// </summary>
        private readonly ArrayList usedList = new ArrayList();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IPList"/> class. 
        /// </summary>
        public IPList()
        {
            // Initialize IP mask list and create IPArrayList into the ipRangeList
            uint mask = 0x00000000;
            for (var level = 1; level < 33; level++)
            {
                mask = (mask >> 1) | 0x80000000;
                this.maskList.Add(mask, level);
                this.ipRangeList.Add(new IPArrayList(mask));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a single IP number to the list as a string, ex. 10.1.1.1
        /// </summary>
        /// <param name="ipNumber">
        /// The ip number.
        /// </param>
        public void Add(string ipNumber)
        {
            this.Add(this.parseIP(ipNumber));
        }

        /// <summary>
        /// Add a single IP number to the list as a unsigned integer, ex. 0x0A010101
        /// </summary>
        /// <param name="ip">
        /// The ip.
        /// </param>
        public void Add(uint ip)
        {
            ((IPArrayList)this.ipRangeList[31]).Add(ip);
            if (!this.usedList.Contains(31))
            {
                this.usedList.Add(31);
                this.usedList.Sort();
            }
        }

        /// <summary>
        /// Adds IP numbers using a mask for range where the mask specifies the number of
        ///   fixed bits, ex. 172.16.0.0 255.255.0.0 will add 172.16.0.0 - 172.16.255.255
        /// </summary>
        /// <param name="ipNumber">
        /// The ip number.
        /// </param>
        /// <param name="mask">
        /// The mask.
        /// </param>
        public void Add(string ipNumber, string mask)
        {
            this.Add(this.parseIP(ipNumber), this.parseIP(mask));
        }

        /// <summary>
        /// Adds IP numbers using a mask for range where the mask specifies the number of
        ///   fixed bits, ex. 0xAC1000 0xFFFF0000 will add 172.16.0.0 - 172.16.255.255
        /// </summary>
        /// <param name="ip">
        /// The ip.
        /// </param>
        /// <param name="umask">
        /// The umask.
        /// </param>
        public void Add(uint ip, uint umask)
        {
            var level = this.maskList[umask];
            if (level != null)
            {
                ip = ip & umask;
                ((IPArrayList)this.ipRangeList[(int)level - 1]).Add(ip);
                if (!this.usedList.Contains((int)level - 1))
                {
                    this.usedList.Add((int)level - 1);
                    this.usedList.Sort();
                }
            }
        }

        /// <summary>
        /// Adds IP numbers using a mask for range where the mask specifies the number of
        ///   fixed bits, ex. 192.168.1.0/24 which will add 192.168.1.0 - 192.168.1.255
        /// </summary>
        /// <param name="ipNumber">
        /// The ip number.
        /// </param>
        /// <param name="maskLevel">
        /// The mask level.
        /// </param>
        public void Add(string ipNumber, int maskLevel)
        {
            this.Add(this.parseIP(ipNumber), (uint)this.maskList.GetKey(this.maskList.IndexOfValue(maskLevel)));
        }

        /// <summary>
        /// Adds IP numbers using a from and to IP number. The method checks the range and
        ///   splits it into normal ip/mask blocks.
        /// </summary>
        /// <param name="fromIp">
        /// From IP.
        /// </param>
        /// <param name="toIp">
        /// To IP.
        /// </param>
        public void AddRange(string fromIp, string toIp)
        {
            this.AddRange(this.parseIP(fromIp), this.parseIP(toIp));
        }

        /// <summary>
        /// Adds IP numbers using a from and to IP number. The method checks the range and
        ///   splits it into normal ip/mask blocks.
        /// </summary>
        /// <param name="fromIp">
        /// From IP.
        /// </param>
        /// <param name="toIp">
        /// To IP.
        /// </param>
        public void AddRange(uint fromIp, uint toIp)
        {
            // If the order is not ascending, switch the IP numbers.
            if (fromIp > toIp)
            {
                var tempIp = fromIp;
                fromIp = toIp;
                toIp = tempIp;
            }

            if (fromIp == toIp)
            {
                this.Add(fromIp);
            }
            else
            {
                var diff = toIp - fromIp;
                var diffLevel = 1;
                var range = 0x80000000;
                if (diff < 256)
                {
                    diffLevel = 24;
                    range = 0x00000100;
                }

                while (range > diff)
                {
                    range = range >> 1;
                    diffLevel++;
                }

                var mask = (uint)this.maskList.GetKey(this.maskList.IndexOfValue(diffLevel));
                var minIp = fromIp & mask;
                if (minIp < fromIp)
                {
                    minIp += range;
                }

                if (minIp > fromIp)
                {
                    this.AddRange(fromIp, minIp - 1);
                    fromIp = minIp;
                }

                if (fromIp == toIp)
                {
                    this.Add(fromIp);
                }
                else
                {
                    if ((minIp + (range - 1)) <= toIp)
                    {
                        this.Add(minIp, mask);
                        fromIp = minIp + range;
                    }

                    if (fromIp == toIp)
                    {
                        this.Add(toIp);
                    }
                    else
                    {
                        if (fromIp < toIp)
                        {
                            this.AddRange(fromIp, toIp);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if an IP number is contained in the lists, ex. 10.0.0.1
        /// </summary>
        /// <param name="ipNumber">
        /// The ip number.
        /// </param>
        /// <returns>
        /// The check number.
        /// </returns>
        public bool CheckNumber(string ipNumber)
        {
            return this.CheckNumber(this.parseIP(ipNumber));
            
        }

        /// <summary>
        /// Checks if an IP number is contained in the lists, ex. 0x0A000001
        /// </summary>
        /// <param name="ip">
        /// The ip.
        /// </param>
        /// <returns>
        /// The check number.
        /// </returns>
        public bool CheckNumber(uint ip)
        {
            var found = false;
            var i = 0;
            while (!found && i < this.usedList.Count)
            {
                found = ((IPArrayList)this.ipRangeList[(int)this.usedList[i]]).Check(ip);
                i++;
            }

            return found;
        }

        /// <summary>
        /// Clears all lists of IP numbers
        /// </summary>
        public void Clear()
        {
            foreach (int i in this.usedList)
            {
                ((IPArrayList)this.ipRangeList[i]).Clear();
            }

            this.usedList.Clear();
        }

        /// <summary>
        /// Generates a list of all IP ranges in printable format
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            var buffer = new StringBuilder();
            foreach (int i in this.usedList)
            {
                buffer.Append("\r\nRange with mask of ").Append(i + 1).Append("\r\n");
                buffer.Append(this.ipRangeList[i].ToString());
            }

            return buffer.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parse a String IP address to a 32 bit unsigned integer
        ///   We can't use System.Net.IPAddress as it will not parse
        ///   our masks correctly eg. 255.255.0.0 is parsed as 65535 !
        /// </summary>
        /// <param name="ipNumber">
        /// The IP number.
        /// </param>
        /// <returns>
        /// The parse ip.
        /// </returns>
        private uint parseIP(string ipNumber)
        {
            uint res = 0;
            var elements = ipNumber.Split(new[] { '.' });
            if (elements.Length == 4)
            {
                res = (uint)Convert.ToInt32(elements[0]) << 24;
                res += (uint)Convert.ToInt32(elements[1]) << 16;
                res += (uint)Convert.ToInt32(elements[2]) << 8;
                res += (uint)Convert.ToInt32(elements[3]);
            }

            return res;
        }

        #endregion
    }

    /// <summary>
    /// Class for storing a range of IP numbers with the same IP mask
    ///   Copyright by Bo Norgaard, All rights reserved.
    ///   Modified for Appleseed by Jeremy Esland (jes1111) 26/04/2005
    /// </summary>
    public sealed class IPArrayList
    {
        #region Constants and Fields

        /// <summary>
        /// The ip num list.
        /// </summary>
        private readonly ArrayList ipNumList = new ArrayList();

        /// <summary>
        /// The ipmask.
        /// </summary>
        private readonly uint ipmask;

        /// <summary>
        /// The is sorted.
        /// </summary>
        private bool isSorted;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IPArrayList"/> class. 
        /// Constructor that sets the mask for the list
        /// </summary>
        /// <param name="mask">
        /// The mask.
        /// </param>
        public IPArrayList(uint mask)
        {
            this.ipmask = mask;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   The IP mask for this list of IP numbers
        /// </summary>
        /// <value>The mask.</value>
        public uint Mask
        {
            get
            {
                return this.ipmask;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new IP number (range) to the list
        /// </summary>
        /// <param name="ipNum">
        /// The IP num.
        /// </param>
        public void Add(uint ipNum)
        {
            this.isSorted = false;
            this.ipNumList.Add(ipNum & this.ipmask);
        }

        /// <summary>
        /// Checks if an IP number is within the ranges included by the list
        /// </summary>
        /// <param name="ipNum">
        /// The IP num.
        /// </param>
        /// <returns>
        /// The check.
        /// </returns>
        public bool Check(uint ipNum)
        {
            var found = false;
            if (this.ipNumList.Count > 0)
            {
                if (!this.isSorted)
                {
                    this.ipNumList.Sort();
                    this.isSorted = true;
                }

                ipNum = ipNum & this.ipmask;
                if (this.ipNumList.BinarySearch(ipNum) >= 0)
                {
                    found = true;
                }
            }

            return found;
        }

        /// <summary>
        /// Clears the list
        /// </summary>
        public void Clear()
        {
            this.ipNumList.Clear();
            this.isSorted = false;
        }

        /// <summary>
        /// The ToString is overridden to generate a list of the IP numbers
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            var buf = new StringBuilder();
            foreach (uint ipnum in this.ipNumList)
            {
                if (buf.Length > 0)
                {
                    buf.Append("\r\n");
                }

                buf.Append(((int)ipnum & 0xFF000000) >> 24).Append('.');
                buf.Append(((int)ipnum & 0x00FF0000) >> 16).Append('.');
                buf.Append(((int)ipnum & 0x0000FF00) >> 8).Append('.');
                buf.Append((int)ipnum & 0x000000FF);
            }

            return buf.ToString();
        }

        #endregion
    }
}