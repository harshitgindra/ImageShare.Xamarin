using System;
using System.Threading.Tasks;

namespace ImageShare
{
    public interface IShareImage
    {
        Task Share(string url);
    }
}
