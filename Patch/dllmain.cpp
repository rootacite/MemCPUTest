// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"
#include <cstdio>
#include <vector>
using namespace std;

extern "C" __declspec(dllexport) unsigned long long getAvailablePhysMemBytes()
{
    static unsigned long long size = 0;

    MEMORYSTATUSEX memoryInfo;
    memoryInfo.dwLength = sizeof(memoryInfo);
    GlobalMemoryStatusEx(&memoryInfo);
    size = memoryInfo.ullAvailPhys;//)/1024/1024
    return size;
}


vector<LPVOID> mems(0);

extern "C" __declspec(dllexport) 
unsigned long long Test(){
 
  

    void* n_point = NULL;
    do {
        n_point = malloc(1024*1024);
        mems.push_back(n_point);
    } while (n_point != NULL && getAvailablePhysMemBytes ()>1024*1024*10);

    unsigned long long r;
    r = mems.size();

    return r;
}


extern "C" __declspec(dllexport) void FreeMems() {

    unsigned long long r;
    r = mems.size();
    for (int i = 0; i < r; i++) {
        free(mems[i]);
    }
}
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

