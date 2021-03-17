using System;
using Microsoft.AspNetCore.Mvc;

namespace Hexarc.Pact.Demo.Api.Controllers
{
    [ApiController, Route("Primitives")]
    public sealed class PrimitivesController : ControllerBase
    {
        [HttpGet, Route(nameof(GetBoolean))]
        public Boolean GetBoolean() => true;

        [HttpGet, Route(nameof(GetByte))]
        public Byte GetByte() => 1;

        [HttpGet, Route(nameof(GetSByte))]
        public SByte GetSByte() => 1;

        [HttpGet, Route(nameof(GetChar))]
        public Char GetChar() => 'A';

        [HttpGet, Route(nameof(GetInt16))]
        public Int16 GetInt16() => -128;

        [HttpGet, Route(nameof(GetUInt16))]
        public UInt16 GetUInt16() => 128;

        [HttpGet, Route(nameof(GetInt32))]
        public Int32 GetInt32() => -256;

        [HttpGet, Route(nameof(GetUInt32))]
        public UInt32 GetUInt32() => 256;

        [HttpGet, Route(nameof(GetInt64))]
        public Int64 GetInt64() => -512;

        [HttpGet, Route(nameof(GetUInt64))]
        public UInt64 GetUInt64() => 256;

        [HttpGet, Route(nameof(GetSingle))]
        public Single GetSingle() => 1.0f;

        [HttpGet, Route(nameof(GetDouble))]
        public Double GetDouble() => 1.0;

        [HttpGet, Route(nameof(GetGuid))]
        public Guid GetGuid() => Guid.NewGuid();

        [HttpGet, Route(nameof(GetDateTime))]
        public DateTime GetDateTime() => DateTime.Now;
    }
}
