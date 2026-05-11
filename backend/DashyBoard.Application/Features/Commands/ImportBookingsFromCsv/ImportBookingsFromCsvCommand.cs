using DashyBoard.Application.Common.Models;
using DashyBoard.Application.DTOs;
using MediatR;

namespace DashyBoard.Application.Features.Commands.ImportBookingsFromCsv;

public record ImportBookingsFromCsvCommand(Stream CsvStream) : IRequest<Result<CsvImportResultDto>>;
