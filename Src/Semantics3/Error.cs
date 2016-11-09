﻿#region License
//The MIT License

//Copyright (c) 2013 Semantics3, Inc. https://semantics3.com

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
/******************************************************************************
 *
 * Name:     Error.cs
 * Project:  Semantics3 API C# Client
 * Purpose:  Class to handle Semantics3 API Errors.
 * Author:   Srinivasan Kidambi, srinivas@semantics3.com
 *
 ******************************************************************************/
#endregion

using System;

namespace Semantics3
{
    public class Error : Exception
    {
        private String type;
        private String message;
        private String param;

        public Error(String _type, String _message, String _param)
        {
            type = _type;
            message = _message;
            param = _param;
        }

        public void BUILD()
        {
            String msg = "Error: " + type + "\nMessage: " + message + "\nFailed at parameter: " + param;
            throw new Exception(msg);
        }
    }
}
