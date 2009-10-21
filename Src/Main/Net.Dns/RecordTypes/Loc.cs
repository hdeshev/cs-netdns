/*
Version 2, June 1991

Copyright (C) 1989, 1991 Free Software Foundation, Inc.  
51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA

Everyone is permitted to copy and distribute verbatim copies
of this license document, but changing it is not allowed.

A full copy of the license can be obtained at: http://www.gnu.org/licenses/gpl.txt 
*/
using System;
using System.Net;

namespace Net.Dns
{
	/// <summary>
	/// An LOC (Physical Location) Resource Record (RR) (RFC1035 3.3.9)
	/// </summary>
	[Serializable]
	public class Loc : IRecord
	{
		private  byte version;
		private  double size;
		private  double hPrecision;
		private  double vPrecision;

		private int latdeg, latmin, latsec, latsecfrag;
		private int longdeg, longmin, longsec, longsecfrag;
		private char northsouth, eastwest;
		private int altmeters, altfrag, altsign;

		private  int latitude;
		private  int longitude;
		private  int altitude;

		public byte Version { get { return this.version; } }
		public double Size { get { return this.size; } }
		public double HoritontalPrecision { get { return hPrecision; } }
		public string HorizontalPrecisionReadable { get { return precsize_ntoa((byte) hPrecision); } }
		public double VerticalPrecision { get { return vPrecision; } }

		public int Latitude { get { return latitude; } }
		public int Longitude { get { return this.longitude; } }
		public int Altitude { get { return this.altitude; } }

		public int LatitudeDegrees { get { return this.latdeg; } }
		public int LatitudeMinutes { get { return this.latmin; } }
		public int LatitudeSeconds { get { return this.latsec; } }
		public int LatitudeMiliSeconds { get { return this.latsecfrag; } }

		public int LongitudeDegrees { get { return this.longdeg; } }
		public int LongitudeMinutes { get { return this.longmin; } }
		public int LongitudeSeconds { get { return this.longsec; } }
		public int LongitudeMiliSeconds { get { return this.longsecfrag; } }

		public int AltidudeMeters { get { return this.altmeters; } }
		public int AltitudeCentimeters { get { return this.altfrag; } }



				
		/// <summary>
		/// Constructs an LOC record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
		public Loc(Pointer pointer)
		{

			int referenceAlt = 100000 * 100;
			int latval, longval, altval;

			version = pointer.ReadByte();
			size = pointer.ReadByte();
			hPrecision = pointer.ReadByte();
			vPrecision = pointer.ReadByte();

			latitude = pointer.ReadInt();
			longitude = pointer.ReadInt();
			altitude = pointer.ReadInt();

			latval = latitude - (1 << 31);
			longval = longitude - (1 << 31);

			if (altitude < referenceAlt)
			{
				//below WGS 84 spheroid 
				altval = referenceAlt - altitude;
				altsign = -1;
			}
			else
			{
				altval = altitude - referenceAlt;
				altsign = 1;
			}

			if (latval < 0)
			{
				northsouth = 'S';
				latval = -latval;
			}
			else
				northsouth = 'N';
			
			//latitude calculation
			latsecfrag = latval % 1000;
			latval /= 1000;
			latsec = latval % 60;
			latval /= 60;
			latmin = latval % 60;
			latval /= 60;
			latdeg = latval;

			if (longval < 0)
			{
				eastwest = 'W';
				longval = -longval;
			}
			else
				eastwest = 'E';

			//longitude calculation
			longsecfrag = longval % 1000;
			longval /= 1000;
			longsec = longval % 60;
			longval /= 60;
			longmin = longval % 60;
			longval /= 60;
			longdeg = longval;

			//altitude
			altfrag = altval % 100;
			altmeters = (altval / 100) * altsign;

		}

		//port from C-code directory of RFC 1876 
		private string precsize_ntoa(byte prec)
		{
			uint[] poweroften = new uint[] { 1, 10, 100, 1000, 10000, 100000, 1000000, 
											   10000000, 100000000, 1000000000 };
			ulong val;
			int mantissa, exponent;

			mantissa = (int) ((prec >> 4) & 0x0f) % 10;
			exponent = (int) ((prec >> 0) & 0x0f) % 10;

			val = (ulong) mantissa * poweroften[exponent];

			return string.Format("{0}.{1}m", (val / 100).ToString("0"), (val % 100).ToString("00"));

		}


		public override string ToString()
		{
			return string.Format("Latitude: {0}° {1}'{2}.{3} {4}, Longitude: {5}° {6}'{7}.{8} {9}, Altitude: {10}.{11}m, Size: {12}, Precision h:{13}, Precision v:{14}", 
				latdeg, latmin.ToString("00"), latsec.ToString("00"), latsecfrag.ToString("000"), northsouth,
				longdeg, longmin.ToString("00"), longsec.ToString("00"), longsecfrag.ToString("000"), eastwest,
				altmeters, altfrag, precsize_ntoa((byte) size), precsize_ntoa((byte) hPrecision), precsize_ntoa((byte)vPrecision));
			//return string.Format("L:{0} l:{1} A:{2} \n\t Size:{3} Precision (h:{4}, v:{5})", latitude, longitude, altitude, size, hPrecision, vPrecision);
		}
	}
}
