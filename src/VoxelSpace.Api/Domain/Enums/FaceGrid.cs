namespace VoxelSpace.Api.Domain.Enums;

/// <summary>
/// Faces do bounding box do espaço de projeto onde uma condição de contorno
/// pode ser aplicada. Equivale aos literais "x_min".."z_max" do engine.
/// </summary>
public enum FaceGrid
{
    XMin = 0,
    XMax = 1,
    YMin = 2,
    YMax = 3,
    ZMin = 4,
    ZMax = 5
}
