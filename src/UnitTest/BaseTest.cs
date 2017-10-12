﻿using DownsizeNet;
using Xunit;

namespace UnitTest
{
    public class BaseTest
    {
        [Fact]
        public void GhostTest()
        {
            var excerpt =
                "Hey! Welcome to Ghost, it's great to have you :) We know that first impressions are important, so we've populated your new site with some initial Getting Started posts that will help you get familiar with everything in no time. This is the first one! There are a few things that you should know up-front: Ghost is designed for ambitious, professional publishers who want to actively build a business around their content. That's who it works best for. If you're using Ghost for some other purpose, that's fine too - but it might not be the best choice for you. The entire platform can be modified and customized to suit your needs, which is very powerful, but doing so does require some knowledge of code. Ghost is not necessarily a good platform for beginners or people who just want a simple personal blog. For the best experience we recommend downloading the Ghost Desktop App for your computer, which is the best way to access your Ghost site on a desktop device. Ghost is made by an independent non-profit organisation called the Ghost Foundation. We are 100% self funded by revenue from our Ghost(Pro) service, and every penny we make is re-invested into funding further development of free, open source technology for modern journalism. The main thing you'll want to read about next is probably: the Ghost editor. Once you're done reading, you can simply delete the default Ghost user from your team to remove all of these introductory posts! ";
            var options = new DownsizeOptions(words: 26);
            var result = Downsize.Substring(excerpt, options);

            Assert.Equal(
                "Hey! Welcome to Ghost, it's great to have you :) We know that first impressions are important, so we've populated your new site with some initial Getting",
                result);
        }
    }
}