cmake_minimum_required(VERSION 2.8.11)
project (day4)

add_executable(
    day4
    main.cpp
    input.txt
)

if (MSVC)
	set(CMAKE_CXX_FLAGS_RELEASE "/Ot /GL /GS- /Gr")
	set(CMAKE_EXE_LINKER_FLAGS_RELEASE "/LTCG")
endif()
set_property(TARGET day4 PROPERTY CXX_STANDARD 17)
