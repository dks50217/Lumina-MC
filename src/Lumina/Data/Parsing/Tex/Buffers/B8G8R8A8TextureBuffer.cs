using System;

namespace Lumina.Data.Parsing.Tex.Buffers
{
    /// <summary>
    /// Represent a face in .tex file, in B8G8R8A8 texture format.
    /// </summary>
    public class B8G8R8A8TextureBuffer : TextureBuffer
    {
        /// <inheritdoc />
        public B8G8R8A8TextureBuffer( bool isDepthConstant, int width, int height, int depth, int[] mipmapAllocations, byte[] buffer )
            : base( isDepthConstant, width, height, depth, mipmapAllocations, buffer )
        {
        }

        /// <inheritdoc />
        public override int NumBytesOfMipmapPerPlane( int mipmapIndex ) => NumPixelsOfMipmapPerPlane( mipmapIndex ) * 4;

        /// <inheritdoc />
        protected override void ConvertToB8G8R8A8( byte[] buffer, int destOffset, int sourceOffset, int width, int height, int depth )
            => Buffer.BlockCopy( RawData, sourceOffset, buffer, destOffset, width * height * depth * 4 );

        /// <inheritdoc />
        protected override TextureBuffer CreateNew( bool isDepthConstant, int width, int height, int depth, int[] mipmapAllocations, byte[] buffer )
            => new B8G8R8A8TextureBuffer( isDepthConstant, width, height, depth, mipmapAllocations, buffer );
    }
}