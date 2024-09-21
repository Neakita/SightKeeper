namespace SightKeeper.Application.Linux.X11;

internal struct XImage
{
	public int width;
	public int height;
	public int xoffset;
	public int format;
	public IntPtr data;
	public int byte_order;
	public int bitmap_unit;
	public int bitmap_bit_order;
	public int bitmap_pad;
	public int depth;
	public int bytes_per_line;
	public int bits_per_pixel;
	public ulong red_mask;
	public ulong green_mask;
	public ulong blue_mask;
	public IntPtr obdata;

	private struct funcs
	{
		private IntPtr create_image;
		private IntPtr destroy_image;
		private IntPtr get_pixel;
		private IntPtr put_pixel;
		private IntPtr sub_image;
		private IntPtr add_pixel;
	}
}