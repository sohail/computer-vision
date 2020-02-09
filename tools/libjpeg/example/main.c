/* 
example/main.c
Cooked by, Sohail Qayum Malik, with the help of borrowed recipes
*/

/*
// For Reading
https://stackoverflow.com/questions/5616216/need-help-in-reading-jpeg-file-using-libjpeg
// For Writing
http://www.andrewewhite.net/wordpress/2008/09/02/very-simple-jpeg-writer-in-c-c/

jpeg_set_quality(), defined in jcparam.c. I was worried about "quality" parameter passed to it. A comment taken taken from the definition of the function...
         "Convert user 0-100 rating to percentage scaling"
*/

#include <stdio.h>
#include <errno.h>

#include "jpeglib.h"
#include "jerror.h"

/* '_errno' : inconsistent dll linkage */
/* They are non-conformant in that they declare "extern int errno" rather than
including errno.h to get the declaration. */
/* extern int errno; */

void read_jpeg_file(const char *name)
{
    printf("Reading file %s\n", name);
}

int main(int argc, char *argv[])
{
    unsigned long x, y;
    int channels;  //  3 =>RGB   4 =>RGBA 
    unsigned long data_size;  // length of the file    
    unsigned char *jdata_input;  // data for the image
    unsigned char *rowptr[1];  // pointer to an array

    JSAMPROW rowptr_output;	/* pointer to JSAMPLE row[s] */

    struct jpeg_decompress_struct dinfo; // For our input jpeg file
    struct jpeg_compress_struct cinfo; // For out output jpeg file

    struct jpeg_error_mgr err, cerr;  // The error handler
     
    if (argc < 2)
    {
        printf("Example usage %s <jpeg file>", argv[0]);
        return 0;
    }

    FILE *file_input = fopen(argv[1], "rb");

    if (file_input == NULL)
    {
        perror("error:");
        return errno;
    }

    dinfo.err = jpeg_std_error(&err);
    jpeg_create_decompress(&dinfo);

    jpeg_stdio_src(&dinfo, file_input);    
    jpeg_read_header(&dinfo, TRUE); // Read jpeg file header

    jpeg_start_decompress(&dinfo);

    // Set width, height
    x = dinfo.output_width;
    y = dinfo.output_height;

    channels = dinfo.num_components;

    printf("%s, (Width = %ld, Height = %ld), channels = ", argv[1], x, y);

    if (channels == 3)
    {
        printf("RGB\n");
        data_size = x * y * 3;
    }
    else if (channels == 4)
    {
        printf("RGBA\n");
        data_size = x * y * 4;
    }
    else 
    {
        data_size = 0;
        printf("unknown\n");
    }

    jdata_input = (unsigned char *)malloc(data_size);

    /*for (int i = 0; i < data_size; i++)
    {
        jdata_input[i] = 0;
    }

    for (int i = 0; i < data_size; i++)
    {
        printf("%d ", jdata_input[i]);
    }*/

    //--------------------------------------------
    // read scanlines one at a time & put bytes 
    // in jdata[] array. Assumes an RGB image
    //--------------------------------------------
    //printf("scanline -> %ld\n", info.output_scanline);
    while (dinfo.output_scanline < dinfo.output_height) // loop
    {
        //printf("scanline -> %ld\n", dinfo.output_scanline + 1);

        // Enable jpeg_read_scanlines() to fill our jdata array
        rowptr[0] = (unsigned char *)jdata_input + (data_size/(x*y))*dinfo.output_width*dinfo.output_scanline;
        jpeg_read_scanlines(&dinfo, rowptr, 1);
    }

    /*
    for (int i = 0; i < y; i++)
    {
        for (int j = 0; j < x; j++)
        {
            printf("%d ", jdata_input[i*x*3 + j]);
        }

    }
    */

    jpeg_finish_decompress(&dinfo);  //finish decompressing    
    jpeg_destroy_decompress(&dinfo);

    fclose(file_input);

    /*for (int i = 0; i < y; i++)
    {
        for (int j = 0; j < x; j++)
        {
            printf("%d ", jdata_input[i*x*3 + j]);
        }
    }*/

    if (argc > 2)
    {
        FILE *file_output = fopen(argv[2], "wb");

        if (file_output == NULL)
        {
            perror("error:");            
        } 
        else
        {   
            cinfo.err = jpeg_std_error(&cerr);
            jpeg_create_compress(&cinfo);
            jpeg_stdio_dest(&cinfo, file_output);
            
            cinfo.image_width = x;
            cinfo.image_height = y;
            cinfo.input_components = (data_size/(x*y));
            cinfo.in_color_space = JCS_RGB;
        
            jpeg_set_defaults(&cinfo);
            jpeg_set_quality(&cinfo, 100, TRUE /* limit to baseline-JPEG values */);
            jpeg_start_compress(&cinfo, TRUE);
            
            while (cinfo.next_scanline < cinfo.image_height)
            {                
                /* jpeg_write_scanlines expects an array of pointers to scanlines.
                 * Here the array is only one element long, but you could pass
                 * more than one scanline at a time if that's more convenient.
                 */
                rowptr_output = (JSAMPROW)(jdata_input + (data_size/(x*y))*x*cinfo.next_scanline);
                jpeg_write_scanlines(&cinfo, &rowptr_output, 1);
                //rowptr[0] = (jdata_input + cinfo.next_scanline*x*(data_size/(x*y)));
                //(void) jpeg_write_scanlines(&cinfo, rowptr, 1);
            }

            jpeg_finish_compress(&cinfo);
            jpeg_destroy_compress(&cinfo);

            fclose(file_output);
        }               
    }


    free(jdata_input); 

    return 0;
}